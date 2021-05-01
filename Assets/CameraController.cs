using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    // Initialized via Boat.cs
    //public Transform Boat;
    public Boat boat;
    public float cameraLead = 1f;
    // Update is called once per frame
    void Update()
    {
        if (boat == null)
        {
            boat = GameObject.FindGameObjectWithTag("Boat").GetComponent<Boat>();
        }
        else
        {
            Vector2 finalVector = Vector2.zero;
            finalVector.Set(boat.steering.moveVector.x, boat.steering.moveVector.y);
            finalVector *= cameraLead;
            Vector3 target = boat.transform.position + new Vector3(finalVector.x, finalVector.y, transform.position.z);
            transform.position = Vector3.Lerp(transform.position, target, 0.1f);
        }
    }
    // TODO add a camera zoom/pan to lighthouse method.
}
