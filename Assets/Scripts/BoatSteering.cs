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
    public bool atHelm = true;

    public Vector2 moveVector;
    [SerializeField]
    float moveMagnitude;

    public Wind wind;
    public Sail sail;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        /*
        //Test Script: Open/Close sail with Left Click
        if (Input.GetButtonDown("Fire1"))
        {
            sail.OpenClose();
        }
        //Test Script: Break/Fix rudder with Right Click
        */
        if (Input.GetButtonDown("Fire2"))
        {
            RudderBreakFix();
        }
        //Ship turning
        if (rudderWorking && atHelm)
        {
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.back * rotation * turnSpeed * Time.deltaTime);
            }
        }
        sailOpen = sail.open;
        //Forward Motion: Move forward if sail is up, lerp towards new move vector
        Vector3 targetVector = sailOpen ? transform.up * baseSpeed * Time.deltaTime : Vector3.zero;
        moveVector = Vector3.Lerp(moveVector, targetVector, accelerationLerp * Time.deltaTime);
        moveMagnitude = moveVector.magnitude;
        float windDotSail = Vector2.Dot(wind.windVector.normalized, sail.transform.up.normalized);
        float windEffect = sailOpen ? 1 - Mathf.Abs(windDotSail) : 0;
        if (sailOpen) { sail.UpdateWind(wind.windVector.normalized);}
        Vector2 windVector = windEffect * wind.windVector * Time.deltaTime;
        Vector2 finalVector = windVector + moveVector;
        //Debug.Log(finalVector);
        transform.position = new Vector3(transform.position.x + finalVector.x, transform.position.y + finalVector.y, transform.position.z);
    }

    public void RudderBreakFix()
    {
        rudderWorking = !rudderWorking;
    }
}
