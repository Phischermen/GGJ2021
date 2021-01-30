using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteering : MonoBehaviour
{
    public float baseSpeed = 10;
    public float turnSpeed = 5;
    [SerializeField]
    bool rudderWorking = true;
    [SerializeField]
    bool sailOpen = true;

    public Vector2 moveVector;
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
        if (sailOpen)
        {
            moveVector = transform.up * baseSpeed * Time.deltaTime;
            transform.position = new Vector3(transform.position.x + moveVector.x, transform.position.y + moveVector.y, transform.position.z);
        }
        if (rudderWorking)
        {
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.back * rotation * turnSpeed * Time.deltaTime);
            }
        }
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
