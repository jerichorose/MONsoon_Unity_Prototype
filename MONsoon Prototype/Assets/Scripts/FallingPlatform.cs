using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public Transform BottomPoint;
    public float FallingSpeed;
    Vector3 TopPoint;

    private void Start()
    {
        TopPoint = transform.position;
    }

    private void Update()
    {
        float fallSpeed = FallingSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, BottomPoint.position, fallSpeed);
        if(transform.position == BottomPoint.position)
        {
            transform.position = TopPoint;
        }
    }
}
