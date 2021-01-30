using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteering : MonoBehaviour
{
    public float baseSpeed = 10;
    public float turnSpeed = 5;
    public float accelerationLerp = .2f;
    [SerializeField]
    bool rudderWorking = true;
    [SerializeField]
    bool sailOpen = true;

    public Vector2 moveVector;
    [SerializeField]
    float moveMagnitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Test Script: Open/Close sail with Left Click
        if (Input.GetButtonDown("Fire1"))
        {
            SailOpenClose();
        }
        //Test Script: Break/Fix rudder with Right Click
        if (Input.GetButtonDown("Fire2"))
        {
            RudderBreakFix();
        }
        //Ship turning
        if (rudderWorking)
        {
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.back * rotation * turnSpeed * Time.deltaTime);
            }
        }
        //Forward Motion: Move forward if sail is up, lerp towards new move vector
        Vector3 targetVector = sailOpen ? transform.up * baseSpeed * Time.deltaTime : Vector3.zero;
        moveVector = Vector3.Lerp(moveVector, targetVector, accelerationLerp * Time.deltaTime);
        moveMagnitude = moveVector.magnitude;
        transform.position = new Vector3(transform.position.x + moveVector.x, transform.position.y + moveVector.y, transform.position.z);
    }

    public void SailOpenClose()
    {
        sailOpen = !sailOpen;
    }

    public void RudderBreakFix()
    {
        rudderWorking = !rudderWorking;
    }
}
