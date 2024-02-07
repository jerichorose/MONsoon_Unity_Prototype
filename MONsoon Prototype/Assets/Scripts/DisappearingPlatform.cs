using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformState { Idle, Touched, Gone}
public class DisappearingPlatform : MonoBehaviour
{
    public float timeToDissapear;
    public float timeToReload;

    bool waitingForDissapear;
    bool waitingForReload;

    float dissapearTimer;
    float reloadTimer;

    void Update()
    {
        if (waitingForDissapear)
        {
            dissapearTimer += Time.deltaTime;
            if(dissapearTimer >= timeToDissapear)
            {
                GetComponent<BoxCollider>().enabled = false;
                GetComponent<MeshRenderer>().enabled = false;
                waitingForDissapear = false;
                waitingForReload = true;
                dissapearTimer = 0;
            }
        }
        else if (waitingForReload)
        {
            reloadTimer += Time.deltaTime;
            if(reloadTimer >= timeToReload)
            {
                GetComponent<BoxCollider>().enabled = true;
                GetComponent<MeshRenderer>().enabled = true;
                waitingForReload = false;
                reloadTimer = 0;
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Player" && !waitingForDissapear)
        {
            waitingForDissapear = true;
        }
    }
}
