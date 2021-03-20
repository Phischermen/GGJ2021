using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatSteering : MonoBehaviour
{
    public float baseSpeed = 10;
    public float turnSpeed = 50f;
    public float rudderBrokenTurnSpeed = 10f;
    public float accelerationLerp = 20f;
    public float brakeLerp = 1f;
    [SerializeField]
    bool rudderWorking = true;
    [SerializeField]
    bool sailOpenAndNotTorn = true;
    public bool atHelm = true;

    public Vector2 moveVector;
    [SerializeField]
    float moveMagnitude;

    public Boat boat;
    public Wind wind;

    public AudioSource audioSource;

    public AudioClip breakClip;
    public AudioClip repairClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GameObject.Find("RudderAudio").GetComponent<AudioSource>();
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
        
        if (Input.GetButtonDown("Fire2"))
        {
            RudderBreakFix();
        }
        */
        //Ship turning
        if (atHelm)
        {
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.back * rotation * (rudderWorking ? turnSpeed : rudderBrokenTurnSpeed) * Time.deltaTime);
            }
        }
        sailOpenAndNotTorn = boat.sail.open && !boat.sail.torn;
        //Forward Motion: Move forward if sail is up, lerp towards new move vector
        Vector3 targetVector = sailOpenAndNotTorn ? transform.up * baseSpeed * Time.deltaTime : Vector3.zero;
        moveVector = Vector3.Lerp(moveVector, targetVector, (!boat.sail.torn ? accelerationLerp : brakeLerp) * Time.deltaTime);
        moveMagnitude = moveVector.magnitude;
        var windVector = (wind != null) ? wind.windVector : Vector2.zero;
        float windDotSail = Vector2.Dot(windVector.normalized, boat.sail.transform.up.normalized);
        float windEffect = sailOpenAndNotTorn ? 1 - Mathf.Abs(windDotSail) : 0;
        if (sailOpenAndNotTorn) { boat.sail.UpdateWind(windVector.normalized); }
        windVector = windEffect * windVector * Time.deltaTime;
        Vector2 finalVector = windVector + moveVector;
        //Debug.Log(finalVector);
        transform.position = new Vector3(transform.position.x + finalVector.x, transform.position.y + finalVector.y, transform.position.z);
    }

    public void RudderBreakFix()
    {
        rudderWorking = !rudderWorking;
    }
    public void RudderFix()
    {
        if (!rudderWorking)
        {
            rudderWorking = true;
            // Play repair
            audioSource.PlayOneShot(repairClip);
        }
    }
    public void RudderBreak()
    {
        if (rudderWorking)
        {
            rudderWorking = false;
            // Play break noise
            audioSource.PlayOneShot(breakClip);
        }
    }
}
