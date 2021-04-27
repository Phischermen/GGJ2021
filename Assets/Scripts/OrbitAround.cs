using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitAround : MonoBehaviour
{

    public GameObject pivot;
    public float speed = 45;

    // Update is called once per frame
    void Update()
    {
        Orbit();
    }

    void Orbit()
    {
        transform.RotateAround(pivot.transform.position, Vector3.forward, speed * Time.deltaTime);

        transform.Rotate(new Vector3(0f, 0f, (speed * Time.deltaTime)*-1));

    }

}
