using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : MonoBehaviour
{
    new AudioSource audio;
    new public AudioClip light;
    public AudioClip douse;
    SpriteMask illum;
    public bool lit = true;

    // Start is called before the first frame update
    void Start()
    {
        illum = GetComponent<SpriteMask>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (illum.enabled != lit) { illum.enabled = lit; }
    }

    public void Light()
    {
        audio.PlayOneShot(light);
        lit = true;
        illum.enabled = true;
    }

    public void Extinguish()
    {
        audio.PlayOneShot(douse);
        lit = false;
        illum.enabled = false;
    }
}
