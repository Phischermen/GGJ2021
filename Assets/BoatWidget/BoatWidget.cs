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

    [System.Serializable]
    public class WidgetImage
    {
        public Image image;
        public WidgetSpriteVariants variations;
    }
    [System.Serializable]
    public class WidgetAnimatedImage
    {
        public ImageAnimation animatedImage;
        public WidgetSpriteVariants[] variations;
    }
    [System.Serializable]
    public class WidgetSpriteVariants
    {

        public Sprite lit;
        public Sprite dark;
        public Sprite pitchBlack;
    }

    [SerializeField]
    public WidgetImage[] widgetImages;
    [SerializeField]
    public WidgetAnimatedImage[] widgetAnimatedImages;

    private WidgetAnimatedImage MurphyWalkWidgetAnimatedImage;
    [SerializeField]
    public WidgetSpriteVariants[] MurpheyWalkingUp;
    [SerializeField]
    public WidgetSpriteVariants[] MurpheyWalkingDown;
    // Here goes nothing!
    public GameObject MurpheyRudder;
    public GameObject MurpheyHelm;
    public GameObject MurpheySail;
    public GameObject MurpheyLantern;
    public GameObject MurpheyTend;
    public GameObject MurpheyWalk;
    List<Sprite> WidgetImages;

    // Initializes outside of BoatWidget
    [HideInInspector]
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
        MurpheyPosition = MurpheyWalk.GetComponent<RectTransform>();
        MurpheyHelm.SetActive(true);
        MurpheyWalk.SetActive(false);
        MurpheyRudder.SetActive(false);
        MurpheySail.SetActive(false);
        MurpheyTend.SetActive(false);
        MurpheyLantern.SetActive(false);

        targetDeckPosition = helmStationPosition;
        currentDeckPosition = helmStationPosition;
        RudderButton = GameObject.Find("Rudder").GetComponent<Button>();
        HelmButton = GameObject.Find("Helm").GetComponent<Button>();
        SailButton = GameObject.Find("Sail").GetComponent<Button>();
        LanternButton = GameObject.Find("Lantern").GetComponent<Button>();
        TendButton = GameObject.Find("Tend").GetComponent<Button>();

        // Get walking animated image. It is assumed to be the first element of widget animated images.
        MurphyWalkWidgetAnimatedImage = widgetAnimatedImages[0];
        // Setup positions
        deckTop = SailButton.transform.position.y;
        deckBottom = RudderButton.transform.position.y;

        SetStationPosition(ref helmStationPosition, HelmButton);
        SetStationPosition(ref tendStationPosition, TendButton);
        SetStationPosition(ref lanternStationPosition, LanternButton);

        // Setup callbacks
        RudderButton.onClick.AddListener(RudderButtonPressed);
        RudderButton.onClick.AddListener(AnyButtonPressed);
        HelmButton.onClick.AddListener(HelmButtonPressed);
        HelmButton.onClick.AddListener(AnyButtonPressed);
        SailButton.onClick.AddListener(SailButtonPressed);
        SailButton.onClick.AddListener(AnyButtonPressed);
        LanternButton.onClick.AddListener(LanternButtonPressed);
        LanternButton.onClick.AddListener(AnyButtonPressed);
        TendButton.onClick.AddListener(TendButtonPressed);
        TendButton.onClick.AddListener(AnyButtonPressed);
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
        MurphyWalkWidgetAnimatedImage.variations = Mathf.Sign(stationChangeCurrentSpeed) == -1f ? MurpheyWalkingDown : MurpheyWalkingUp;
        Debug.Log(currentDeckPosition);
        MurpheyPosition.position = new Vector2(MurpheyPosition.position.x, Mathf.Lerp(deckBottom, deckTop, currentDeckPosition));
        if (currentDeckPosition == targetDeckPosition && currentStation != targetStation)
        {
            currentStation = targetStation;
            Debug.Log("Station Reached: " + currentStation);
            MurpheyWalk.SetActive(false);
            MurpheyTend.SetActive(false);
            MurpheyLantern.SetActive(false);
            switch (currentStation)
            {
                case Station.helm:
                    MurpheyHelm.SetActive(true);
                    boatSteering.atHelm = true;
                    break;
                case Station.sail:
                    MurpheySail.SetActive(true);
                    boatSteering.sail.RepairSail();
                    boatSteering.sail.atSail = true;
                    break;
                case Station.rudder:
                    MurpheyRudder.SetActive(true);
                    boatSteering.RudderFix();
                    break;
                case Station.lantern:
                    MurpheyLantern.SetActive(true);
                    break;
                case Station.tend:
                    MurpheyTend.SetActive(true);
                    break;
            }
        }
        // Set sprite variants based on light level
        // TODO calculate light level
        var lightLevel = 0.5f;
        foreach (var i in widgetImages)
        {
            if (lightLevel > 0.5f)
            {
                i.image.sprite = i.variations.lit;
            }
            else if (lightLevel > 0.25f)
            {
                i.image.sprite = i.variations.dark;
            }
            else
            {
                i.image.sprite = i.variations.pitchBlack;
            }
        }
        foreach (var ai in widgetAnimatedImages)
        {
            if (ai.animatedImage.sprites.Length != ai.variations.Length)
            {
                Debug.Log(ai.animatedImage + " has incongruent variation and sprite length.");
            }
            if (lightLevel > 0.5f)
            {
                for (var i = 0; i < ai.animatedImage.sprites.Length; i++)
                {
                    ai.animatedImage.sprites[i] = ai.variations[i].lit;
                }
            }
            else if (lightLevel > 0.25f)
            {
                for (var i = 0; i < ai.animatedImage.sprites.Length; i++)
                {
                    ai.animatedImage.sprites[i] = ai.variations[i].dark;
                }
            }
            else
            {
                for (var i = 0; i < ai.animatedImage.sprites.Length; i++)
                {
                    ai.animatedImage.sprites[i] = ai.variations[i].pitchBlack;
                }
            }
        }
        //Debug.Log(currentDeckPosition);
    }
    public void AnyButtonPressed()
    {
        boatSteering.atHelm = false;
        boatSteering.sail.atSail = false;
        MurpheyWalk.SetActive(true);
        MurpheyHelm.SetActive(false);
        MurpheyRudder.SetActive(false);
        MurpheySail.SetActive(false);
        MurpheyTend.SetActive(false);
        MurpheyLantern.SetActive(false);

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
