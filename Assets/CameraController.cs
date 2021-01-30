using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject Boat;
    // Start is called before the first frame update
    void Start()
    {
        Boat = GameObject.FindGameObjectWithTag("Boat");
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Boat.transform.position;
    }
}
