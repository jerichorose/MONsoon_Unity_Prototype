using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHoldPlayer : MonoBehaviour
{
    Transform platformGameObject;
    private void Start()
    {
        platformGameObject = transform.parent.GetComponent<Transform>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "GD")
        {
            if(other.transform.parent.tag == "Player")
            {
                //Debug.Log("you should b working");
                other.transform.parent.parent = platformGameObject;
            }
            else
            {
                //Debug.Log("you fucked up. gameobject name is " + other.gameObject.name);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.tag == "Player")
        {
            //Debug.Log("you should b working");
            other.transform.parent.parent = null;
        }
        else
        {
            //Debug.Log("you fucked up. gameobject name is " + other.gameObject.name);
        }

    }
}
