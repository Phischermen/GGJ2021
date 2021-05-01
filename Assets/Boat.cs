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

    public bool startCrashed;
    [HideInInspector]
    public bool objectiveDelivered;

    public float cameraLead = 5;
    private void Awake()
    {
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

        damageManager.boat = steering.boat = cameraController.boat = this;
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
        //cameraController.boat = GameObject.FindGameObjectWithTag("Lighthouse").transform;
        yield return new WaitForSeconds(5f);
        //cameraController.boat = GameObject.FindGameObjectWithTag("Boat").transform;
    }
    public void TeleportBoat(Vector3 position)
    {
        var offset = cameraController.transform.position - transform.position;
        transform.position = position;
        cameraController.transform.position = transform.position + offset;
    }
}
