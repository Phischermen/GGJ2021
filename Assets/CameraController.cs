using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Initialized via Boat.cs
    public GameObject Boat;

    // Update is called once per frame
    void Update()
    {
        if (Boat == null)
        {
            Boat = GameObject.FindGameObjectWithTag("Boat");
        }
        else
        {
            transform.position = new Vector3(Boat.transform.position.x, Boat.transform.position.y, transform.position.z);
        }
    }
}
