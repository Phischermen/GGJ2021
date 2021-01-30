using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoatWidget : MonoBehaviour
{
    
    Button RudderButton;
    Button HelmButton;
    Button SailButton;
    Button LanternButton;
    Button TendButton;
    // Initializes outside of BoatWidget
    public BoatSteering boatSteering;

    Station currentStation;
    Station targetStation;
    enum Station
    {
        rudder,
        helm,
        sail,
        lantern,
        tend,
        noStation
    }

    readonly float rudderStationPosition = 0f;
    readonly float helmStationPosition = 0.2f;
    readonly float sailStationPosition = 0.4f;
    readonly float lanternStationPosition = 0.6f;
    readonly float tendStationPosition = 0.8f;

    float currentDeckPosition;
    float targetDeckPosition;

    float stationChangeCurrentSpeed = 0f;
    float stationChangeTopSpeed = 0.1f;
    // Start is called before the first frame update
    void Start()
    {
        targetDeckPosition = helmStationPosition;
        currentDeckPosition = helmStationPosition;
        RudderButton = GameObject.Find("Rudder").GetComponent<Button>();
        HelmButton = GameObject.Find("Helm").GetComponent<Button>();
        SailButton = GameObject.Find("Sail").GetComponent<Button>();
        LanternButton = GameObject.Find("Lantern").GetComponent<Button>();
        TendButton = GameObject.Find("Tend").GetComponent<Button>();

        // Setup callbacks
        RudderButton.onClick.AddListener(RudderButtonPressed);
        HelmButton.onClick.AddListener(HelmButtonPressed);
        SailButton.onClick.AddListener(SailButtonPressed);
        LanternButton.onClick.AddListener(LanternButtonPressed);
        TendButton.onClick.AddListener(TendButtonPressed);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(targetDeckPosition - currentDeckPosition) > 0.05f)
        {
            var targetSpeed = Mathf.Sign(currentDeckPosition - targetDeckPosition) == -1f ?
            Mathf.Max(targetDeckPosition - currentDeckPosition, -stationChangeTopSpeed) :
            Mathf.Min(targetDeckPosition - currentDeckPosition, stationChangeTopSpeed);
            stationChangeCurrentSpeed = Mathf.Lerp(stationChangeCurrentSpeed, targetSpeed, 0.1f);
            currentDeckPosition += stationChangeCurrentSpeed * Time.deltaTime;
        }
        else
        {
            currentDeckPosition = targetDeckPosition;
        }
        if (currentDeckPosition == targetDeckPosition && currentStation != targetStation)
        {
            currentStation = targetStation;
            Debug.Log("Station Reached: " + currentStation);
            //switch (currentStation)
            //{
            //    case Station.helm:
            //        if (!boatSteering.manHelm)
            //            boatSteering.ManHelm();
            //        break;
            //    case Station.sail:
            //        if (!boatSteering.manSail)
            //            boatSteering.ManSail();
            //        break;
            //}
        }

        //Debug.Log(currentDeckPosition);
    }

    public void RudderButtonPressed()
    {
        targetStation = Station.rudder;
        currentStation = Station.noStation;
        targetDeckPosition = rudderStationPosition;
    }
    public void HelmButtonPressed()
    {
        targetStation = Station.helm;
        currentStation = Station.noStation;
        targetDeckPosition = helmStationPosition;
    }
    public void SailButtonPressed()
    {
        targetStation = Station.sail;
        currentStation = Station.noStation;
        targetDeckPosition = sailStationPosition;
    }
    public void LanternButtonPressed()
    {
        targetStation = Station.lantern;
        currentStation = Station.noStation;
        targetDeckPosition = lanternStationPosition;
    }
    public void TendButtonPressed()
    {
        targetStation = Station.tend;
        currentStation = Station.noStation;
        targetDeckPosition = tendStationPosition;
    }
}
