using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public List<Transform> Stations;
    public float MoveSpeed;
    public bool WaitAtStation;
    public float StationWaitTime;
    int currentTargetStation;

    float WaitTimer;

    void Start()
    {
        currentTargetStation = 0;
    }

    void Update()
    {
        float moveSpeed = MoveSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Stations[currentTargetStation].position, moveSpeed);
        if(transform.position == Stations[currentTargetStation].position)
        {
            if(WaitAtStation && (WaitTimer < StationWaitTime))
            {
                WaitTimer += Time.deltaTime;
            }
            else
            {
                if (currentTargetStation == (Stations.Count - 1))
                {
                    currentTargetStation = 0;
                }
                else
                {
                    currentTargetStation++;
                }
            }
        }
    }
}
