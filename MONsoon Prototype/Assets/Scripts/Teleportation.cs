using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleportation : MonoBehaviour
{
    public List<TeleportSpots> teleports = new List<TeleportSpots>();

    private void Update()
    {
        for (int i = 0; i < teleports.Count; i++)
        {
            if (Input.GetKey(teleports[i].TeleportKey))
            {
                transform.SetPositionAndRotation(teleports[i].TeleportLocation, transform.rotation);
            }
        }
    }
}

[System.Serializable]
public class TeleportSpots
{
    public string Title;
    public Vector3 TeleportLocation;
    public KeyCode TeleportKey;
}
