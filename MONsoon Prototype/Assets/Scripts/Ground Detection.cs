using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    //Create an int to detect how many different objects the Ground Detection box is touching
    public int ContactCount = 0;


    //Add to ContactCount every time something touches the Ground Detection box
    PlayerController pc;
    private void Start()
    {
        pc = FindObjectOfType<PlayerController>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if ((!other.isTrigger) && (other.gameObject.layer == 6))
        {
            ContactCount++;

            if(!pc.UseNewJump)
            {
                pc.CurrentGravityMode = GravityMode.Rising;
            }
        }

        if(pc.isJumping)
        {
            if(pc.UseNewJump)
            {
                pc.HitGround();
            }
            else
            {
                Physics.gravity = new Vector3(0, pc.RisingGravity, 0);
                pc.CurrentGravityMode = GravityMode.Rising;
            }
            pc.isJumping = false;
        }

    }

    //Subtract from ContactCount every time the Ground Detection box stops touching a certain object
    private void OnTriggerExit(Collider other)
    {
        if (!other.isTrigger && other.gameObject.layer == 6)
            ContactCount--;
        if(!pc.isJumping)
        {
            if(pc.UseNewJump)
            {
                Physics.gravity = new Vector3(0, pc.NewFallingGravity);
            }
            else
            {
                Physics.gravity = new Vector3(0, pc.FallingGravity, 0);
                pc.CurrentGravityMode = GravityMode.Falling;
            }

        }
    }

    //Return whether or not the player is grounded depending on if the Ground Detection box is touching anything 
    public bool IsGrounded()
    {
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
