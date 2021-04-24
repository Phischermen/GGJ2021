using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Boat : MonoBehaviour
{
    public BoatSteering steering;
    public BoatDamageManager damageManager;
    public Lantern lantern;
    public Sail sail;
    public Captain captain;

    public CameraController cameraController;

    public bool startCrashed;
    public bool objectiveDelivered;
    private void Awake()
    {
        // Initialize sub components
        steering = GetComponentInChildren<BoatSteering>();
        damageManager = GetComponentInChildren<BoatDamageManager>();
        lantern = GetComponentInChildren<Lantern>();
        sail = GetComponentInChildren<Sail>();
        captain = GetComponentInChildren<Captain>();

        damageManager.boat = steering.boat = this;

        //Find main camera and attach camera controller script
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        cameraController = camera.AddComponent<CameraController>();
        cameraController.Boat = gameObject;
    }
    public void BindBoatToBoatWidget(BoatWidget widget)
    {
        widget.boat = this;
        captain.anim = widget.GetComponentInChildren<Animator>();
        captain.img = captain.anim.gameObject.GetComponent<Image>();
        damageManager.boatWidget = widget;
        damageManager.SetupDamageActions();
        if (startCrashed)
        {
            widget.CaptainAsleep();
            captain.Sleep(silently: true);
            sail.OpenClose(false, silently: true);
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
    private IEnumerator ShowLighthouse()
    {
        cameraController.Boat = GameObject.FindGameObjectWithTag("Lighthouse");
        yield return new WaitForSeconds(5f);
        cameraController.Boat = GameObject.FindGameObjectWithTag("Boat");
    }
}
