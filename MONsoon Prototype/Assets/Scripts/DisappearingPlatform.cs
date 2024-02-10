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

    float dissapearTimer = 0;
    float reloadTimer = 0;


    void Update()
    {
        //Debug.Log("dissapeartimer = " + dissapearTimer + " and time to dissapear  = " + timeToDissapear);
        if (waitingForDissapear)
        {
            float percentageOfFuck = dissapearTimer / timeToDissapear;
            Debug.Log("percentageOfFuck = " + percentageOfFuck);
            Color bColor = GetComponent<MeshRenderer>().material.color;
            GetComponent<MeshRenderer>().material.color = new Color(bColor.r, bColor.g, bColor.b, Mathf.Lerp(1, 0, dissapearTimer / timeToDissapear));
            Debug.Log(GetComponent<MeshRenderer>().material.color.a);
            dissapearTimer += Time.deltaTime;

            if(dissapearTimer >= timeToDissapear)
            {
                GetComponent<BoxCollider>().enabled = false;
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
                Color bColor = GetComponent<MeshRenderer>().material.color;
                GetComponent<MeshRenderer>().material.color = new Color(bColor.r, bColor.g, bColor.b, 1);
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
