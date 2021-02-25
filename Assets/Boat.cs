using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    // Initialized through inspector
    public BoatSteering steering;
    public BoatDamageManager damageManager;
    public Lantern lantern;
    public Sail sail;
    public Captain captain;

    public void BindBoatToBoatWidget(BoatWidget widget)
    {
        widget.boat = this;
        captain.anim = widget.GetComponentInChildren<Animator>();
    }
}
