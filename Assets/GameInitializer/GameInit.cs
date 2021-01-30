using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject boatPrefab;
    public GameObject lightHousePrefab;
    [HideInInspector]
    public float initialDistance;
    // How long the game would last if the player could drive straight towards the lighthouse.
    public float minimumTravelTime;
    // Initial Angle Between Boat And LightHouse
    public float initABBALH;
    // Start is called before the first frame update
    void Start()
    {
        var boat = Instantiate(boatPrefab);
        var lightHouse = Instantiate(lightHousePrefab);
        var d = GetDistanceTraveledOverTime(boat.GetComponent<BoatSteering>().baseSpeed, minimumTravelTime);

        boat.transform.position = Vector3.zero;
        lightHouse.transform.position = new Vector3(Mathf.Sin(Mathf.Deg2Rad * initABBALH) * d, Mathf.Cos(Mathf.Deg2Rad * initABBALH) * d, 0f);
        boat.transform.Rotate(0f, 0f, -initABBALH);
    }

    // Update is called once per frame
    //void Update()
    //{

    //}

    public float GetDistanceTraveledOverTime(float speed, float time)
    {
        return speed * time;
    }
}
