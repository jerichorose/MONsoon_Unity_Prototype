using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//aspect of getting funky bellow
public enum GravityMode { Rising, Falling, Top}
public enum CustomForceMode { VelocityChange, Force, Impulse, Acceleration}
public class PlayerController : MonoBehaviour
{
    public float MoveSpeed;
    public float SprintMultiplier;
    public float JumpForce;
    public CustomForceMode MyForceMode = CustomForceMode.Force;
    public Rigidbody rb;
    GroundDetection gd;
    public bool LockCursor;

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
        JumpCheck();
        if(UsingCustomGravity && gd.inAir)
        {
            ApplyGravity();
        }
        SetCam();
        Debug.Log(rb.velocity.y);
    }

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

    void JumpCheck()
    {
        if(Input.GetKeyDown(KeyCode.Space) &&  gd.IsGrounded())
        {
            Vector3 jump = new Vector3(0, JumpForce, 0);
            switch(MyForceMode)
            {
                case CustomForceMode.VelocityChange:
                    rb.AddForce(jump, ForceMode.VelocityChange);
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
            rb.AddForce(jump, ForceMode.VelocityChange);
            //rb.AddForce(jump, ForceMode.Acceleration);
            //rb.AddForce(jump, ForceMode.Force);
            //rb.AddForce(jump, ForceMode.Impulse);
            gd.inAir = true;
        }
    }

    public bool UsingCustomGravity;
   // public float CustomGravity;
    public float RisingGravity;
    public float FallingGravity;

    public bool UsingTopArcGravity;
    public float TopArc;
    public float TopArcGravity;

    // public float GravityScale;
    // public float GlobalGravity = -9.81f;

    //ok im gna try and get funky here. sorry.
    public GravityMode CurrentGravityMode = GravityMode.Rising;

    public float trackingRise;
    public float trackingFalling;
    void ApplyGravity()
    {
        switch (CurrentGravityMode)
        {
            case GravityMode.Rising:
                if (UsingTopArcGravity && (TopArc < rb.velocity.y))
                {
                    Physics.gravity = new Vector3(0, TopArcGravity, 0);
                    CurrentGravityMode = GravityMode.Top;
                    //trackingRise = rb.velocity.y - TopArc;
                }
                else
                {
                    if (rb.velocity.y < trackingRise)
                    {
                        Physics.gravity = new Vector3(0, FallingGravity, 0);
                        CurrentGravityMode = GravityMode.Falling;
                    }
                }
                trackingRise = rb.velocity.y;
                break;
            case GravityMode.Top:
                //trying to find a way to like, check reverse of arc
                if (UsingTopArcGravity && (rb.velocity.y < -TopArc))
                {
                    Physics.gravity = new Vector3(0, FallingGravity, 0);
                    CurrentGravityMode = GravityMode.Falling;
                }
                break;
            case GravityMode.Falling:

                if ((trackingFalling < 0) && (rb.velocity.y == 0))
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

    //Section for the camera values, there are a lot of them. Includes yaw, pitch, zoom, transforms, and sensitivity settings
    #region Camera Values

    //Transforms the Camera needs, assigned in Start()
    Transform YawPivotTransform;
    Transform PitchPivotTransform;
    Transform CameraTransform;

    [Header("Yaw, Horizontal Camera Movement")]
    [Tooltip("Yaw Sensitivity is how sensitive the camera response to horizontal mouse movement is. The minimum value is 100 and the maximum is 1000.")]
    [Range(100, 1000)]
    public float YawSensitivity = 600f;

    //Tracks the current Horizontal Angle
    float YawAngle = 0f;

    [Header("Pitch, Vertical Camera Movement")]
    [Tooltip("Pitch Sensitivity is how sensitive the camera response to vertical mouse movement is. The minimum value is 100 and the maximum is 1000.")]
    [Range(100, 1000)]
    public float PitchSensitivity = 600.0f;

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
