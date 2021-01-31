using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BoatWidget : MonoBehaviour
{
    public float deckTop;
    public float deckBottom;

    public GameObject Murphey;
    RectTransform MurpheyPosition;

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

    float rudderStationPosition = 0f;
    float helmStationPosition = 0.2f;
    float sailStationPosition = 1f;
    float lanternStationPosition = 0.6f;
    float tendStationPosition = 0.8f;

    float currentDeckPosition;
    float targetDeckPosition;

    float stationChangeCurrentSpeed = 0f;
    public float stationChangeMinSpeed = 0.1f;
    public float stationChangeTopSpeed = 0.9f;

    public float stationChangeAcceptableRange = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        MurpheyPosition = Murphey.GetComponent<RectTransform>();
        targetDeckPosition = helmStationPosition;
        currentDeckPosition = helmStationPosition;
        RudderButton = GameObject.Find("Rudder").GetComponent<Button>();
        HelmButton = GameObject.Find("Helm").GetComponent<Button>();
        SailButton = GameObject.Find("Sail").GetComponent<Button>();
        LanternButton = GameObject.Find("Lantern").GetComponent<Button>();
        TendButton = GameObject.Find("Tend").GetComponent<Button>();

        // Setup positions
        deckTop = SailButton.transform.position.y;
        deckBottom = RudderButton.transform.position.y;

        SetStationPosition(ref helmStationPosition, HelmButton);
        SetStationPosition(ref tendStationPosition, TendButton);
        SetStationPosition(ref lanternStationPosition, LanternButton);

        // Setup callbacks
        RudderButton.onClick.AddListener(RudderButtonPressed);
        HelmButton.onClick.AddListener(HelmButtonPressed);
        SailButton.onClick.AddListener(SailButtonPressed);
        LanternButton.onClick.AddListener(LanternButtonPressed);
        TendButton.onClick.AddListener(TendButtonPressed);
    }

    private void SetStationPosition(ref float position, Button button)
    {
        position = (button.transform.position.y - deckBottom) / (deckTop - deckBottom);
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(targetDeckPosition - currentDeckPosition) > stationChangeAcceptableRange)
        {
            var targetSpeed = Mathf.Sign(currentDeckPosition - targetDeckPosition) == -1f ?
            Mathf.Min(Mathf.Max(stationChangeMinSpeed, targetDeckPosition - currentDeckPosition), stationChangeTopSpeed) :
            Mathf.Max(Mathf.Min(-stationChangeMinSpeed, targetDeckPosition - currentDeckPosition), -stationChangeTopSpeed);
            stationChangeCurrentSpeed = Mathf.Lerp(stationChangeCurrentSpeed, targetSpeed, 0.1f);
            currentDeckPosition += stationChangeCurrentSpeed * Time.deltaTime;
        }
        else
        {
            stationChangeCurrentSpeed = 0f;
            currentDeckPosition = targetDeckPosition;
        }
        Debug.Log(currentDeckPosition);
        MurpheyPosition.position = new Vector2(MurpheyPosition.position.x, Mathf.Lerp(deckBottom, deckTop, currentDeckPosition));
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
