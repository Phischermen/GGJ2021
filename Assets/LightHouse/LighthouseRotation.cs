using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LighthouseRotation : MonoBehaviour
{
    public float rotationSpeed = 5;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
