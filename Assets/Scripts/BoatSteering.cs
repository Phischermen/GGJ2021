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

    [HideInInspector]
    public Vector2 moveVector;
    [HideInInspector]
    public Vector2 reboundVector;
    public float reboundRecovery;
    [SerializeField]
    float moveMagnitude;

    public Boat boat;
    public Wind wind;

    public AudioSource audioSource;

    public AudioClip breakClip;
    public AudioClip repairClip;

    private float timeSpentSteeringIncorrectly;
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
        //Ship and rudimentry sail turning
        float rotation = Input.GetAxis("Horizontal");
        if (atHelm)
        {
            float sailRotation = Input.GetAxis("Horizontal2");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.back * rotation * (rudderWorking ? turnSpeed : rudderBrokenTurnSpeed) * Time.deltaTime);
            }
            if (Mathf.Abs(sailRotation) > .1)
            {
                boat.sail.RotateSail(sailRotation, boat.sail.turnSpeedFromHelm);
            }
        }
        else if (!boat.sail.atSail && rotation != 0)
        {
            timeSpentSteeringIncorrectly += Time.deltaTime;
            if (timeSpentSteeringIncorrectly > 2)
            {
                timeSpentSteeringIncorrectly = 0;
                DialogueManager.singleton.DisplayMessage(DialogueManager.Messages.steeringAwayFromWheel);

            }
        }
        else
        {
            timeSpentSteeringIncorrectly = Mathf.Max(0, timeSpentSteeringIncorrectly - Time.deltaTime);
        }
        sailOpenAndNotTorn = boat.sail.open && !boat.sail.torn;
        //Forward Motion: Move forward if sail is up, lerp towards new move vector
        Vector3 targetVector = sailOpenAndNotTorn ? transform.up * baseSpeed : Vector3.zero;
        moveVector = Vector3.Lerp(moveVector, targetVector, (!boat.sail.torn ? accelerationLerp : brakeLerp) * Time.deltaTime);
        moveMagnitude = moveVector.magnitude;
        // Wind
        var windVector = (wind != null) ? wind.windVector : Vector2.zero;
        float windDotSail = Vector2.Dot(windVector.normalized, boat.sail.transform.up.normalized);
        float windEffect = sailOpenAndNotTorn ? 1 - Mathf.Abs(windDotSail) : 0;
        if (sailOpenAndNotTorn) { boat.sail.UpdateWind(windVector.normalized); }
        windVector = windEffect * windVector;
        // Rebound
        reboundVector = Vector2.Lerp(reboundVector, Vector2.zero, reboundRecovery * Time.deltaTime);
        Vector2 finalVector = (windVector + moveVector + reboundVector) * Time.deltaTime;
        //Debug.Log(finalVector);
        transform.position = new Vector3(transform.position.x + finalVector.x, transform.position.y + finalVector.y, transform.position.z);
    }

    public void RudderBreakFix()
    {
        rudderWorking = !rudderWorking;
    }
    public void StartRudderFix()
    {
        // Play repair
        audioSource.PlayOneShot(repairClip);
    }
    public void RudderFix()
    {
        if (!rudderWorking)
        {
            rudderWorking = true;
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

    public string GetRudderStatus(bool control = false)
    {
        if (rudderWorking)
        {
            return "Rudder is in working condition.";
        } else
        {
            return "Rudder is in disrepair.";
        }
    }
}
