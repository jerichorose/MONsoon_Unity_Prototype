using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//aspect of getting funky bellow
public enum GravityMode { Rising, Falling, Top}
public enum CustomForceMode { VelocityChange, Force, Impulse, Acceleration}
public class PlayerController : MonoBehaviour
{
    public bool newJumpSettingsOpen;
    public bool UseNewJump;
    public float NewRisingGravity;
    public float NewFallingGravity;
    public float NewJumpStrength;
    public float NewJumpMinTime;
    public float NewJumpMaxTime;
    public CustomForceMode NewForceMode = CustomForceMode.Impulse;



    public float MoveSpeed;
    public float SprintMultiplier;
    public float JumpForce;
    public CustomForceMode MyForceMode = CustomForceMode.Force;
    public Rigidbody rb;
    public GroundDetection gd;
    public bool LockCursor;

    public bool isJumping = false;

    float dumbfuckingcount = .05f;
    float dumbfuckingtimer = 0;
    bool dumbfuckingcounting = false;

    public bool jumpSettingsOpen;
    public bool cameraSettingsOpen;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        gd = FindObjectOfType<GroundDetection>();

        //Goodbye cursor. this doesnt always work in the unity editor which is annoying, but it always works on build.
        //Your cursor cant move and you cant see it, so it can't distract you.
        if (LockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Find all camera transform objects from scene so that they do not have to be assigned in editor. There is a script attatched to them 
        //that says not to change their names.
        YawPivotTransform = GameObject.Find("YawPivot").transform;
        PitchPivotTransform = GameObject.Find("PitchPivot").transform;
        CameraTransform = GameObject.Find("TPCamera").transform;

        if(UsingCustomGravity)
        {
            Physics.gravity = new Vector3(0, RisingGravity, 0);

        }
    }

    private void Update()
    {
        Move();

        if(UseNewJump)
        {
            NewJumpCheck();
        }
        else
        {
            JumpCheck();
        }

        if(UsingCustomGravity)
        {
            ApplyGravity();
        }
       SetCam();

        if(Input.GetKeyDown(KeyCode.X))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        //this was for like, tryna see how big the jumpy is
        if(UsingTopArcGravity)
        {
            if (trackingRise > transform.position.y)
            {
                topMove = trackingRise;
            }
            trackingRise = transform.position.y;
        }

        if(dumbfuckingcounting)
        {
            dumbfuckingtimer += Time.deltaTime;
            //bool isbeingheld = Input.GetKey(KeyCode.Space);
           // Debug.Log(dumbfuckingtimer + isbeingheld.ToString());
            if(dumbfuckingtimer >= dumbfuckingcount)
            {
                if(!Input.GetKey(KeyCode.Space) && isJumping)
                {
                    //Debug.Log("this works");
                    Physics.gravity = new Vector3(0, FallingGravity, 0);
                    CurrentGravityMode = GravityMode.Falling;
                    dumbfuckingtimer = 0;
                    dumbfuckingcounting = false;
                }
            }
        }

    }

    public float topMove;

    void Move()
    {
        Vector3 dir = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            //Add forward movement to direction value relative to camera's horizontal rotation
            dir += YawPivotTransform.forward;
        }

        if (Input.GetKey(KeyCode.A))
        {
            //Add left movement to direction value relative to camera's horizontal rotation
            dir -= YawPivotTransform.right;
        }

        if (Input.GetKey(KeyCode.S))
        {
            //Add backward movement to direction value relative to camera's horizontal rotation
            dir -= YawPivotTransform.forward;
        }

        if (Input.GetKey(KeyCode.D))
        {
            //Add right movement to direction value relative to camera's horizontal rotation
            dir += YawPivotTransform.right;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            dir = dir.normalized * (MoveSpeed * SprintMultiplier);
        }
        else
        {
            dir = dir.normalized * MoveSpeed;
        }
        //Mulitply the direction by the movespeed to get the correct velocity needed


        //Assign the new dir value to the x and z of the rigidbody, leaving Y alone since it is handled by JumpCheck
        rb.velocity = new Vector3(dir.x, rb.velocity.y, dir.z);
    }

    float newJumpMaxTimer = 0;
    float newJumpMinTimer = 0;
    void NewJumpCheck()
    {
        if(Input.GetKey(KeyCode.Space))
        {
            if(isJumping)
            {
                if(newJumpMinTimer < NewJumpMinTime)
                {
                    newJumpMinTimer += Time.deltaTime;
                }
                else
                {
                    if ((rb.velocity.y > 0) && (newJumpMaxTimer < NewJumpMaxTime))
                    {
                        newJumpMaxTimer += Time.deltaTime;
                    }
                    //(rb.velocity.y < 0) || 
                    else if ((newJumpMaxTimer > NewJumpMaxTime) && Physics.gravity.y != NewFallingGravity)
                    {
                        Physics.gravity = new Vector3(0, FallingGravity);
                    }
                }
            }

            else if(gd.IsGrounded())
            {
                switch (NewForceMode)
                {
                    case CustomForceMode.VelocityChange:
                        rb.AddForce(new Vector3(rb.velocity.x, NewJumpStrength), ForceMode.VelocityChange);
                        break;
                    case CustomForceMode.Force:
                        rb.AddForce(new Vector3(rb.velocity.x, NewJumpStrength), ForceMode.Force);
                        break;
                    case CustomForceMode.Impulse:
                        rb.AddForce(new Vector3(rb.velocity.x, NewJumpStrength), ForceMode.Impulse);
                        break;
                    case CustomForceMode.Acceleration:
                        rb.AddForce(new Vector3(rb.velocity.x, NewJumpStrength), ForceMode.Acceleration);
                        break;
                }
                isJumping = true;

            }
        }
        //ur jumping but not pressing the key anymore so now ur falling
        else if(isJumping)
        {
            if(newJumpMinTimer < NewJumpMinTime)
            {
                newJumpMinTimer += Time.deltaTime;
            }
            else
            {
                Physics.gravity = new Vector3(0, (NewFallingGravity), 0);
            }

        }
    }

    public void HitGround()
    {
        isJumping = false;
        newJumpMaxTimer = 0;
        Physics.gravity = new Vector3(0, (NewRisingGravity), 0);
    }

    void JumpCheck()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&  gd.IsGrounded())
        {
            
            Vector3 jump = new Vector3(0, JumpForce, 0);
            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            //these were also for tryna find jump values
           // Debug.Log("jumping pos x and y " + transform.position.x + " " + transform.position.z);
            //Debug.Log("jumping starting y pos = " + transform.position.y);
            CurrentGravityMode = GravityMode.Rising;
            dumbfuckingcounting = true;

            isJumping = true;

            switch (MyForceMode)
            {
                case CustomForceMode.VelocityChange:
                    rb.AddForce(new Vector3(0, JumpForce), ForceMode.VelocityChange);
                    break;
                case CustomForceMode.Force:
                    rb.AddForce(jump, ForceMode.Force);
                    break;
                case CustomForceMode.Impulse:
                    rb.AddForce(jump, ForceMode.Impulse);
                    break;
                case CustomForceMode.Acceleration:
                    rb.AddForce(jump, ForceMode.Acceleration);
                    break;
            }

        }
    }

    public bool UsingCustomGravity;
    public float RisingGravity;
    public float FallingGravity;

    public bool UsingTopArcGravity;
    public float TopArcRising;
    public float TopArcFalling;
    public float TopArcGravity;

    //ok im gna try and get funky here. sorry.
    public GravityMode CurrentGravityMode = GravityMode.Rising;

    public float trackingRise;
    public float trackingFalling;


    void ApplyGravity()
    {
        if(!gd.IsGrounded())
        {
            switch (CurrentGravityMode)
            {
                case GravityMode.Rising:
                    //Debug.Log("rising y velocity = " + rb.velocity.y);
                    
                    if (UsingTopArcGravity && (TopArcRising < rb.velocity.y))
                    {
                        bool xandyare0 = (rb.velocity.z == 0 && rb.velocity.x == 0);
                        if (xandyare0)
                        {
                            //Debug.Log("short jump");
                            Physics.gravity = new Vector3(0, (TopArcGravity *3), 0);
                            CurrentGravityMode = GravityMode.Top;
                        }
                        else 
                        {
                            //Debug.Log("long jump");
                            Physics.gravity = new Vector3(0, TopArcGravity, 0);
                            CurrentGravityMode = GravityMode.Top;
                        }

                    }
                    else if(!UsingTopArcGravity)
                    {
                        if (rb.velocity.y < trackingRise)
                        {
                            Physics.gravity = new Vector3(0, FallingGravity, 0);
                            CurrentGravityMode = GravityMode.Falling;
                        }
                        trackingRise = rb.velocity.y;
                    }
                    break;
                case GravityMode.Top:
                    //Debug.Log("top arc y velocity = " + rb.velocity.y);
                    if (rb.velocity.y < TopArcFalling)
                    {
                        Physics.gravity = new Vector3(0, FallingGravity, 0);
                        CurrentGravityMode = GravityMode.Falling;
                        //rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
                    }
                    break;
                case GravityMode.Falling:
                    //Debug.Log("falling velocity y = " + rb.velocity.y);
                    if (rb.velocity.y == 0)
                    {
                        Physics.gravity = new Vector3(0, RisingGravity, 0);
                        CurrentGravityMode = GravityMode.Rising;
                    }
                    else
                    {
                        trackingFalling = rb.velocity.y;
                    }
                    break;
            }
        }
    }

    //Section for the camera values, there are a lot of them. Includes yaw, pitch, zoom, transforms, and sensitivity settings
    #region Camera Values

    //Transforms the Camera needs, assigned in Start()
    Transform YawPivotTransform;
    Transform PitchPivotTransform;
    Transform CameraTransform;

    [Header("Yaw, Horizontal Camera Movement")]
    [Tooltip("Yaw Sensitivity is how sensitive the camera response to horizontal mouse movement is. The minimum value is 100 and the maximum is 1000.")]
    [Range(100, 1000)]
    public float YawSensitivity = 400f;

    //Tracks the current Horizontal Angle
    float YawAngle = 0f;

    [Header("Pitch, Vertical Camera Movement")]
    [Tooltip("Pitch Sensitivity is how sensitive the camera response to vertical mouse movement is. The minimum value is 100 and the maximum is 1000.")]
    [Range(100, 1000)]
    public float PitchSensitivity = 300f;

    //Tracks the current Vertical Angle
    float PitchAngle = 0.0f;

    [Header("Minimum and Maximum Angle for Vertical Camera Movement")]
    [Tooltip("Pitch Min is the lowest vertical angle you want the player to be able to move the Camera. Minimum is -90, Maximum is 90. 0 is directly behind player, 90 is directly above player.")]
    [Range(-90, 90)]
    public float PitchMin = 0f;

    [Tooltip("Pitch Max is highest vertical angle you want the player to be able to move the Camera. Minimum is -90, Maximum is 90. 0 is directly behind player, 90 is directly above player.")]
    [Range(-90, 90)]
    public float PitchMax = 85f;


    [Header("Invert Camera Values")]
    [Tooltip("Inverting Pitch means that moving your mouse up moves the camera down, vice versa. Check to invert Pitch, uncheck otherwise.")]
    public bool InvertPitch;

    [Tooltip("Inverting Yaw means that moving your mouse left moves the camera right, vice versa. Check to invert Yaw, uncheck otherwise.")]
    public bool InvertYaw;
    #endregion
    void SetCam()
    {
        #region Horizontal Camera Movement
        YawAngle += Input.GetAxis("Mouse X") * YawSensitivity * Time.deltaTime;
        YawPivotTransform.localRotation = Quaternion.Euler(0, YawAngle, 0);

        //Check if the yaw is supposed to be inverted
        if (InvertYaw)
        {
            //If yaw should be inverted, rotate the yawpivot horizontally based on this angle value
            YawPivotTransform.localRotation = Quaternion.Euler(0.0f, YawAngle, 0.0f);
        }
        else
        {
            //If yaw should not be inverted, rotate the yawpivot horizontally based on the negative version of this angle value
            YawPivotTransform.localRotation = Quaternion.Euler(0.0f, -YawAngle, 0.0f);
        }
        #endregion

        #region Vertical Camera Movement
        //THIS IS VERTICAL! 
        PitchAngle += Input.GetAxis("Mouse Y") * PitchSensitivity * Time.deltaTime;

        //Check if the pitch is supposed to be inverted (I brute forced this and now looking at it gives me a headache)
        if (InvertPitch)
        {
            //If the pitch should be inverted, make sure the pitch angle is between the negative versions of the pitch min and max
            PitchAngle = Mathf.Clamp(PitchAngle, -PitchMax, -PitchMin);
            //Then, change the pitch pivots rotation to a negative version of the pitch angle
            PitchPivotTransform.localRotation = Quaternion.Euler(-PitchAngle, 0.0f, 0.0f);
        }
        else
        {
            //Make sure the pitch angle is between the pitch min and max
            PitchAngle = Mathf.Clamp(PitchAngle, PitchMin, PitchMax);
            //Change the pitch pivot's rotation to the pitch angle
            PitchPivotTransform.localRotation = Quaternion.Euler(PitchAngle, 0.0f, 0.0f);
        }
        #endregion
    }
}
