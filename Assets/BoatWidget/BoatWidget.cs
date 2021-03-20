﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Extensions;
public class BoatWidget : MonoBehaviour
{
    public CanvasRenderer BoatCR;

    [HideInInspector]
    public Lantern lantern;

    [HideInInspector]
    public float deckTop;
    [HideInInspector]
    public float deckBottom;

    public GameObject Murphey;
    RectTransform MurpheyPosition;

    public AudioSource clickSource;
    public AudioSource footStepLoop;

    Button ManualButton;
    
    Button RudderButton;
    Button HelmButton;
    Button SailButton;
    Button LanternButton;
    Button TendButton;

    public Image RudderButtonImage;
    public Image HelmButtonImage;
    public Image SailButtonImage;
    public Image LanternButtonImage;
    public Image TendButtonImage;

    public Sprite RudderNormalSprite;
    public Sprite HelmNormalSprite;
    public Sprite SailNormalSprite;
    public Sprite LanternNormalSprite;
    public Sprite TendNormalSprite;

    public class Station
    {
        public Action OnManStation;
        Action OnRepairStation;
        Image StationButtonImage;
        Image StationButtonInvertedImage;
        Sprite StationButtonSprite;
        public bool repaired = true;
        public float repair = 1f;
        public float repairRate = 0.5f;
        public Station(Action onManStation, Image image, Sprite sprite, bool repairable = false, Action onRepairStation = null, float _repairRate = 0.5f)
        {
            OnManStation = onManStation;
            OnRepairStation = onRepairStation;
            StationButtonImage = image;
            StationButtonSprite = sprite;
            repairRate = _repairRate;

            if (repairable)
            {
                var StationInvertedBackground = new GameObject();
                var rt = StationInvertedBackground.AddComponent<RectTransform>();
                StationInvertedBackground.transform.SetParent(image.transform, worldPositionStays: false);
                rt.MatchOther(image.rectTransform);
                rt.Translate(-Vector3.forward);
                StationButtonInvertedImage = StationInvertedBackground.AddComponent<Image>();
                StationButtonInvertedImage.raycastTarget = false;
                StationButtonInvertedImage.type = Image.Type.Filled;
                StationButtonInvertedImage.fillMethod = Image.FillMethod.Radial360;
                StationButtonInvertedImage.fillAmount = 0f;
                StationButtonInvertedImage.sprite = Resources.Load<Sprite>("Sprites/UI/uiButtons/" + sprite.name + "_invert");
            }
        }
        public void ProgressRepair(float rate)
        {
            repair = Mathf.Clamp(repair + (repairRate * Time.deltaTime * rate), 0f, 1f);
            if (repaired == false && repair == 1f)
            {
                repaired = true;
                if (OnRepairStation != null) OnRepairStation.Invoke();
            }
            StationButtonInvertedImage.fillAmount = 1f - repair;
        }
    }
    public Station[] stations = new Station[(int)StationNames.noStation];

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
    public Boat boat;
    [HideInInspector]
    public InstructionManual manual;

    StationNames currentStation;
    StationNames targetStation;
    public enum StationNames
    {
        rudder,
        helm,
        sail,
        lantern,
        tend,
        noStation // Also represents the number of stations
    }

    // Station positions are set procedurally based on the position of their button
    float rudderStationPosition = 0f;
    float helmStationPosition;
    float sailStationPosition = 1f;
    float lanternStationPosition;
    float tendStationPosition;
    private void SetStationPosition(ref float position, Button button)
    {
        position = (button.transform.position.y - deckBottom) / (deckTop - deckBottom);
    }

    float currentDeckPosition;
    float targetDeckPosition;

    float stationChangeCurrentSpeed = 0f;
    [Header("Station change properties")]
    public float stationChangeMinSpeed = 0.1f;
    public float stationChangeTopSpeed = 0.9f;
    public float stationChangeTravelScale = 2f;
    public float stationChangeAcceptableRange = 0.01f;
    [Header("Repair rates (in Repairs/second)")]
    public float TendRepairRate = 0.5f;
    public float RudderRepairRate = 0.5f;
    public float SailRepairRate = 0.5f;
    public float LanternRepairRate = 0.5f;

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

        targetStation = StationNames.helm;
        currentStation = StationNames.helm;

        // Get Buttons
        RudderButton = GameObject.Find("Rudder").GetComponent<Button>();
        HelmButton = GameObject.Find("Helm").GetComponent<Button>();
        SailButton = GameObject.Find("Sail").GetComponent<Button>();
        LanternButton = GameObject.Find("Lantern").GetComponent<Button>();
        TendButton = GameObject.Find("Tend").GetComponent<Button>();
        ManualButton = GameObject.Find("Help").GetComponent<Button>();

        // Get walking animated image. It is assumed to be the first element of widget animated images.
        MurphyWalkWidgetAnimatedImage = widgetAnimatedImages[0];

        // Setup stations
        stations[(int)StationNames.rudder] = new Station(RudderManned, RudderButtonImage, RudderNormalSprite, true, RepairRudder, RudderRepairRate);
        stations[(int)StationNames.helm] = new Station(HelmManned, HelmButtonImage, HelmNormalSprite);
        stations[(int)StationNames.sail] = new Station(SailManned, SailButtonImage, SailNormalSprite, true, RepairSail, SailRepairRate);
        stations[(int)StationNames.lantern] = new Station(ManLantern, LanternButtonImage, LanternNormalSprite, true, RepairLantern, LanternRepairRate);
        stations[(int)StationNames.tend] = new Station(ManTend, TendButtonImage, TendNormalSprite, true, RepairTend, TendRepairRate);

        // Setup station positions
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

        ManualButton.onClick.AddListener(ManualButtonPressed);
    }

    void Update()
    {
        // Check input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HelmButton.onClick.Invoke();
        }
        // Move Murphey across the deck
        var delta = (targetDeckPosition - currentDeckPosition) * stationChangeTravelScale;
        if (Mathf.Abs(delta) > stationChangeAcceptableRange)
        {
            var targetSpeed = Mathf.Sign(delta) == 1f ?
            // Target is above Murphey
                Mathf.Clamp(delta, stationChangeMinSpeed, stationChangeTopSpeed) :
            // Target is below Murphey
                Mathf.Clamp(delta, -stationChangeTopSpeed, -stationChangeMinSpeed);
            stationChangeCurrentSpeed = Mathf.Lerp(stationChangeCurrentSpeed, targetSpeed, 0.1f);
            currentDeckPosition += stationChangeCurrentSpeed * Time.deltaTime;
        }
        else // Murphey is close enough to man his targeted station. 
        {
            stationChangeCurrentSpeed = 0f;
            // Set current to target to trigger station reached logic.
            currentDeckPosition = targetDeckPosition;
        }
        MurpheyPosition.position = new Vector2(MurpheyPosition.position.x, Mathf.Lerp(deckBottom, deckTop, currentDeckPosition));
        // Make Murphey face the direction he is walking.
        MurphyWalkWidgetAnimatedImage.variations = Mathf.Sign(stationChangeCurrentSpeed) == -1f ? MurpheyWalkingDown : MurpheyWalkingUp;
        // Check if station reached.
        if (currentDeckPosition == targetDeckPosition && currentStation != targetStation)
        {
            currentStation = targetStation;
            MurpheyWalk.SetActive(false);
            footStepLoop.Stop();
            stations[(int)currentStation].OnManStation.Invoke();
        }
        // Loop through stations
        for (var i = 0; i < (int)StationNames.noStation; i++)
        {
            var s = stations[(int)i];
            if (!s.repaired)
            {
                if (i == (int)currentStation)
                {
                    s.ProgressRepair(1f);
                }
                else
                {
                    s.ProgressRepair(-0.1f);
                }
            }
        }
        // Set sprite variants based on light level
        // TODO calculate light level
        var lightLevel = 1f;
        if (lantern != null)
        {
            lightLevel = (lantern.lit) ? 1f : 0.5f;
        }
        BoatCR.SetColor(Color.Lerp(Color.black, Color.white, lightLevel));
        // Iterate through images
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
        // Iterate through animated images
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
    }
    #region Damage Event UI Response Methods
    public void SailTorn()
    {
        stations[(int)StationNames.sail].repaired = false;
        stations[(int)StationNames.sail].repair = 0f;
    }
    public void LanternExtinguished()
    {

        stations[(int)StationNames.lantern].repaired = false;
        stations[(int)StationNames.lantern].repair = 0f;
    }

    public void RudderBroke()
    {
        stations[(int)StationNames.rudder].repaired = false;
        stations[(int)StationNames.rudder].repair = 0f;
    }

    public void CaptainAsleep()
    {
        stations[(int)StationNames.tend].repaired = false;
        stations[(int)StationNames.tend].repair = 0f;
    }
    #endregion
    #region Button Callbacks
    public void AnyButtonPressed()
    {
        clickSource.Play();
        footStepLoop.Play();
        if (boat)
        {
            boat.steering.atHelm = false;
            boat.sail.atSail = false;
        }
        MurpheyWalk.SetActive(true);
        MurpheyHelm.SetActive(false);
        MurpheyRudder.SetActive(false);
        MurpheySail.SetActive(false);
        MurpheyTend.SetActive(false);
        MurpheyLantern.SetActive(false);

        currentStation = StationNames.noStation;
    }
    public void RudderButtonPressed()
    {
        RudderButtonImage.sprite = RudderNormalSprite;
        targetStation = StationNames.rudder;
        targetDeckPosition = rudderStationPosition;
    }
    public void HelmButtonPressed()
    {
        HelmButtonImage.sprite = HelmNormalSprite;
        targetStation = StationNames.helm;
        targetDeckPosition = helmStationPosition;
    }
    public void SailButtonPressed()
    {
        SailButtonImage.sprite = SailNormalSprite;
        targetStation = StationNames.sail;
        targetDeckPosition = sailStationPosition;
    }
    public void LanternButtonPressed()
    {
        LanternButtonImage.sprite = LanternNormalSprite;
        targetStation = StationNames.lantern;
        targetDeckPosition = lanternStationPosition;
    }
    public void TendButtonPressed()
    {
        TendButtonImage.sprite = TendNormalSprite;
        targetStation = StationNames.tend;
        targetDeckPosition = tendStationPosition;
    }
    public void ManualButtonPressed()
    {
        // Close instead of open if manual already open
        if (manual.open)
        {
            CloseManualPressed();
            return;
        }
        manual.OpenManual();
    }
    public void CloseManualPressed()
    {
        manual.CloseManual();
    }
    #endregion
    #region Station Manned Callbacks
    public void HelmManned()
    {
        MurpheyHelm.SetActive(true);
        if (boat) boat.steering.atHelm = true;
    }
    public void SailManned()
    {
        MurpheySail.SetActive(true);
        if (boat) boat.sail.atSail = true;
    }
    public void RudderManned()
    {
        MurpheyRudder.SetActive(true);
    }
    public void ManLantern()
    {
        MurpheyLantern.SetActive(true);
    }
    public void ManTend()
    {
        MurpheyTend.SetActive(true);
    }
    #endregion
    #region Station Repaired Callbacks
    public void RepairRudder()
    {
        boat.steering.RudderFix();
    }
    public void RepairSail()
    {
        boat.sail.RepairSail();
    }
    public void RepairLantern()
    {
        boat.lantern.Light();
    }
    public void RepairTend()
    {
        boat.captain.Wake();
    }
    #endregion
}
