using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatformHoldPlayer : MonoBehaviour
{
    Transform platformGameObject;
    private void Start()
    {
        platformGameObject = GetComponentInParent<Transform>();
    }
    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            other.transform.parent = platformGameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.parent = null;
    }
}
