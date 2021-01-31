using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInit : MonoBehaviour
{
    public GameObject gameMasterPrefab;
    public GameObject boatPrefab;
    public GameObject lightHousePrefab;
    public GameObject boatWidgetInScene;
    public GameObject windPrefab;
    [HideInInspector]
    public float initialDistance;
    // How long the game would last if the player could drive straight towards the lighthouse.
    public float minimumTravelTime;
    // Initial Angle Between Boat And LightHouse
    public float initABBALH;
    // Start is called before the first frame update
    void Start()
    {
        var gameMaster = Instantiate(gameMasterPrefab);
        var boat = Instantiate(boatPrefab);
        var boatSteering = boat.GetComponent<BoatSteering>();
        var obstacleSpawner = boat.AddComponent<ObstacleSpawner>();
        var wind = Instantiate(windPrefab);
        obstacleSpawner.obstacleGrid = gameObject.AddComponent<Grid>();
        obstacleSpawner.obstacleGrid.cellSize = new Vector3(10, 10);

        var boatWidget = boatWidgetInScene.GetComponent<BoatWidget>();
        boatWidget.boatSteering = boatSteering;
        boatWidget.lantern = boat.GetComponentInChildren<Lantern>();

        var boatDamageManager = boat.GetComponent<BoatDamageManager>();
        boatDamageManager.boatWidget = boatWidget;
        boatDamageManager.gameMaster = gameMaster.GetComponent<GameMaster>();
        boatDamageManager.SetupDamageActions();

        var lightHouse = Instantiate(lightHousePrefab);
        lightHouse.GetComponentInChildren<Harbor>().gameMaster = gameMaster.GetComponent<GameMaster>();
        var d = GetDistanceTraveledOverTime(boatSteering.baseSpeed, minimumTravelTime);

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
