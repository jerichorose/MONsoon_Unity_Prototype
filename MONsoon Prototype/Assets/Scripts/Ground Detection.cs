using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    //Create an int to detect how many different objects the Ground Detection box is touching
    public int ContactCount = 0;
    public bool inAir = true;
    //Add to ContactCount every time something touches the Ground Detection box
    PlayerController pc;
    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger)
        {
            ContactCount++;
        }
        if(inAir)
        {
            inAir = false;
            if(pc.CurrentGravityMode == GravityMode.Falling)
            {
                Physics.gravity = new Vector3(0, pc.RisingGravity, 0);
                pc.CurrentGravityMode = GravityMode.Rising;
                pc.rb.velocity = new Vector3(pc.rb.velocity.x, 0, pc.rb.velocity.z);
            }
        }
    }

    //Subtract from ContactCount every time the Ground Detection box stops touching a certain object
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger)
            ContactCount--;
    }

    //Return whether or not the player is grounded depending on if the Ground Detection box is touching anything 
    public bool IsGrounded()
    {
        Debug.Log(ContactCount);
        if (ContactCount > 0)
        {
            Physics.gravity = new Vector3(0, pc.RisingGravity, 0);
            pc.CurrentGravityMode = GravityMode.Rising;
            return true;
        }
        else
        {
            return false;
        }
    }
}
