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
    private void Start()
    {
        // Initialize sub components
        steering = GetComponentInChildren<BoatSteering>();
        damageManager = GetComponentInChildren<BoatDamageManager>();
        lantern = GetComponentInChildren<Lantern>();
        sail = GetComponentInChildren<Sail>();
        captain = GetComponentInChildren<Captain>();

        steering.boat = this;

        //Find main camera and attach camera controller script
        var camera = GameObject.FindGameObjectWithTag("MainCamera");
        var cameraController = camera.AddComponent<CameraController>();
        cameraController.Boat = gameObject;
    }
    public void BindBoatToBoatWidget(BoatWidget widget)
    {
        widget.boat = this;
        captain.anim = widget.GetComponentInChildren<Animator>();
        captain.img = captain.anim.gameObject.GetComponent<Image>();
        damageManager.boatWidget = widget;
        damageManager.SetupDamageActions();
    }
}
