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
    public InstructionManual instructionManualInScene;
    [HideInInspector]
    public float initialDistance;
    // How long the game would last if the player could drive straight towards the lighthouse.
    public float minimumTravelTime;
    // Initial Angle Between Boat And LightHouse
    public float initABBALH;
    // Start is called before the first frame update
    void Start()
    {
        // Instantiate game master
        var gameMaster = Instantiate(gameMasterPrefab);
        // Instantiate and setup boat
        var boat = Instantiate(boatPrefab);
        var boatComponent = boat.GetComponent<Boat>();
        // Instantiate and setup Obstacle Spawner
        var obstacleSpawner = boat.AddComponent<ObstacleSpawner>();
        obstacleSpawner.obstacleGrid = gameObject.AddComponent<Grid>();
        obstacleSpawner.obstacleGrid.cellSize = new Vector3(10, 10);
        // Instantiate wind
        // Wind attaches itself to boat when instantiated
        var wind = Instantiate(windPrefab);
        // Setup boat widget
        // Boat widget is assigned through inspector
        var boatWidget = boatWidgetInScene.GetComponent<BoatWidget>();
        boatComponent.BindBoatToBoatWidget(boatWidget);
        boatComponent.damageManager.gameMaster = gameMaster.GetComponent<GameMaster>();
        // Setup instruction manual
        boatWidget.manual = instructionManualInScene;
        boatWidget.manual.preGameVersion = false;
        boatWidget.manual.OnNextPressedOnLastPage += boatWidget.CloseManualPressed;
        // Instantiate and setup light house
        var lightHouse = Instantiate(lightHousePrefab);
        lightHouse.GetComponentInChildren<Harbor>().gameMaster = gameMaster.GetComponent<GameMaster>();
        var d = GetDistanceTraveledOverTime(boatComponent.steering.baseSpeed, minimumTravelTime);
        // Position boat and light house.
        // TODO place boat in the middle of obstacle grid, and place light house at an offset from this position.
        boat.transform.position = Vector3.zero;
        lightHouse.transform.position = new Vector3(Mathf.Sin(Mathf.Deg2Rad * initABBALH) * d, Mathf.Cos(Mathf.Deg2Rad * initABBALH) * d, 0f);
        boat.transform.Rotate(0f, 0f, -initABBALH);
    }

    public float GetDistanceTraveledOverTime(float speed, float time)
    {
        return speed * time;
    }
}
