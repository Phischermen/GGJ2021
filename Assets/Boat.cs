using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{
    [HideInInspector]
    public BoatSteering steering;
    [HideInInspector]
    public BoatDamageManager damageManager;
    [HideInInspector]
    public Lantern lantern;
    [HideInInspector]
    public Sail sail;
    [HideInInspector]
    public Captain captain;

    [HideInInspector]
    public CameraController cameraController;
    public BoatWidget boatWidget;
    private static bool hasPlayedTutorial;
    public bool startCrashed;
    [HideInInspector]
    public bool objectiveDelivered;
    [HideInInspector]
    public bool sailRaisedForFirstTime;

    public AnimationCurve cameraToLightHouse;
    public float cameraLead = 5;
    private void Awake()
    {
        if (!Application.isEditor) startCrashed = !hasPlayedTutorial;
        // Initialize sub components
        steering = GetComponentInChildren<BoatSteering>();
        damageManager = GetComponentInChildren<BoatDamageManager>();
        lantern = GetComponentInChildren<Lantern>();
        sail = GetComponentInChildren<Sail>();
        captain = GetComponentInChildren<Captain>();
        //Find main camera and attach camera controller script
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = camera.AddComponent<CameraController>();
        cameraController.cameraLead = cameraLead;
        // Position camera a little behind boat initially
        cameraController.transform.position = new Vector3(transform.position.x - 5f, transform.position.y - 5f, cameraController.transform.position.z);

        damageManager.boat = sail.boat = steering.boat = cameraController.boat = this;
    }
    public void BindBoatToBoatWidget(BoatWidget widget)
    {
        boatWidget = widget;
        widget.boat = this;
        captain.anim = widget.GetComponentInChildren<Animator>();
        captain.img = captain.anim.gameObject.GetComponent<Image>();
        captain.boatWidget = widget;

        damageManager.boatWidget = widget;
        damageManager.SetupDamageActions();
        if (startCrashed)
        {
            widget.stations[(int)BoatWidget.StationNames.rudder].HideSelf();
            widget.stations[(int)BoatWidget.StationNames.sail].HideSelf();
            widget.stations[(int)BoatWidget.StationNames.lantern].HideSelf();
            widget.CaptainAsleep();
            captain.Sleep(silently: true);
            sail.OpenClose(false, silently: true);
            widget.LanternExtinguished();
            lantern.Extinguish(silently: true);
        }
    }
    public void DeliverObjective()
    {
        if (startCrashed && objectiveDelivered == false)
        {
            objectiveDelivered = true;
            DialogueManager.singleton.DisplayMessage(DialogueManager.Messages.gameStart);
            StartCoroutine(ShowLighthouse());
        }
    }
    public void RaiseSailForFirstTime()
    {
        sailRaisedForFirstTime = true;
        hasPlayedTutorial = true;
        foreach (var station in boatWidget.stations)
        {
            if (station.hidden)
            {
                station.RevealSelf();
            }
        }
    }
    private IEnumerator ShowLighthouse()
    {
        var progress = 0f;
        var camera = cameraController.GetComponent<Camera>();
        var lighthouse = GameObject.FindGameObjectWithTag("Lighthouse");
        cameraController.enabled = false;
        while (progress < 0.99f)
        {
            progress += Time.deltaTime / 2f;
            progress = Mathf.Clamp01(progress);
            var t = cameraToLightHouse.Evaluate(progress);
            Vector3 v = Vector3.Lerp(transform.position, lighthouse.transform.position, t);
            v.z = -10;
            camera.transform.position = v;
            camera.orthographicSize = Mathf.Lerp(5, 20, t);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(2f);
        progress = 0f;
        while (progress < 0.99f)
        {
            progress += Time.deltaTime / 2f;
            progress = Mathf.Clamp01(progress);
            var t = cameraToLightHouse.Evaluate(progress);
            Vector3 v = Vector3.Lerp(lighthouse.transform.position, transform.position, t);
            v.z = -10;
            camera.transform.position = v;
            camera.orthographicSize = Mathf.Lerp(20, 5, t);
            yield return new WaitForEndOfFrame();
        }
        cameraController.enabled = true;
        camera.orthographicSize = 5;
    }
    public void TeleportBoat(Vector3 position)
    {
        var offset = cameraController.transform.position - transform.position;
        transform.position = position;
        cameraController.transform.position = transform.position + offset;
    }
}
