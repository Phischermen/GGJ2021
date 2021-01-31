﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    SpriteMask illum;
    public bool lit = true;

    // Start is called before the first frame update
    void Start()
    {
        illum = GetComponent<SpriteMask>();
        transform.parent.GetComponent<BoatSteering>().lantern = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (illum.enabled != lit) { illum.enabled = lit; }
    }

    public void Light()
    {
        lit = true;
        illum.enabled = true;
    }

    public void Extinguish()
    {
        lit = false;
        illum.enabled = false;
    }
}
