﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    public Boat boat;

    public bool open = true;
    public bool torn = false;
    public bool atSail = false;

    
    enum Fill
    {
        Low,
        Mid,
        Full,
        Closed,
        Torn,
    }

    private Fill currentFill;
    
    [HideInInspector]
    public bool rotatingSail = false;
    public float turnSpeed = 5;
    public float turnSpeedFromHelm = 5;
    public SailSprite lightSail;
    public SailSprite darkSail;

    [Range(0, 1)]
    public float midSailMin = .33f;
    [Range(0, 1)]
    public float fullSailMin = .66f;

    [SerializeField]
    private bool flipSail = false;

    public AudioSource sailRotateLoop;

    public AudioSource sailFillLoop;

    public AudioClip fullSailLoop;
    public AudioClip tornSailLoop;

    public AudioSource sailBreakAudioSource;
    public AudioClip sailTear;
    public AudioClip sailRepair;
    public AudioClip sailRaise;
    public AudioClip sailLower;
    // Start is called before the first frame update
    void Start()
    {
        sailFillLoop = GameObject.Find("SailFillAudio").GetComponent<AudioSource>();
        sailRotateLoop.Play();
        sailRotateLoop.Pause();
    }

    // Update is called once per frame
    void Update()
    {
        if (atSail)
        {
            float raiselower = Input.GetAxis("Vertical");
            if (Mathf.Abs(raiselower) > .1)
            {
                bool opening = raiselower > 0 ? true : false;
                if (opening != open) OpenClose(opening);
                if (opening && !boat.sailRaisedForFirstTime)
                {
                    boat.RaiseSailForFirstTime();
                }
            }
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                RotateSail(rotation, turnSpeed);
            }
        }
        // Only play sail rotation if sail is rotating
        if (rotatingSail)
        {
            rotatingSail = false;
            sailRotateLoop.UnPause();
        }
        else
        {
            sailRotateLoop.Pause();
        }
    }

    public void RotateSail(float rotation, float speed)
    {
        rotatingSail = true;
        transform.Rotate(Vector3.forward * rotation * speed * Time.deltaTime);
        float rotationZ = transform.localRotation.eulerAngles.z;
        rotationZ = rotationZ > 180 ? Mathf.Clamp(rotationZ, 270, 360) : Mathf.Clamp(rotationZ, 0, 90);
        //Debug.Log("pre clamp: " + transform.localRotation.eulerAngles.z + "post clamp: " + rotationZ);
        transform.localRotation = Quaternion.Euler(0, 0, rotationZ);
    }

    public void OpenClose()
    {
        Fill fill = open ? Fill.Closed : Fill.Low;
        SetSprites(fill);
        open = !open;
    }

    public void OpenClose(bool opening, bool silently = false)
    {
        Fill fill = opening ? Fill.Low : Fill.Closed;
        SetSprites(fill);
        open = opening;
        if (silently) return;
        if (opening)
        {
            sailBreakAudioSource.Stop();
            sailBreakAudioSource.PlayOneShot(sailRaise);
        }
        else
        {
            sailBreakAudioSource.Stop();
            sailBreakAudioSource.PlayOneShot(sailLower);
        }
    }
    public void TearSail()
    {
        if (!torn)
        {

            // Switch to torn sail loop
            sailFillLoop.clip = tornSailLoop;
            sailFillLoop.Play();
            // Play tear sail sound
            sailBreakAudioSource.PlayOneShot(sailTear);
            torn = true;
            SetSprites(Fill.Torn);
        }
    }
    public void RepairSail()
    {
        if (torn)
        {
            // Switch to full sail loop
            sailFillLoop.clip = fullSailLoop;
            sailFillLoop.Play();
            // Play repair sail sound
            sailBreakAudioSource.PlayOneShot(sailRepair);
            torn = false;
        }
    }

    public void UpdateWind(Vector2 windVect)
    {
        if (transform.up.y * windVect.x > transform.up.x * windVect.y)
        {
            if (flipSail != false)
            {
                flipSail = false;
                FlipSprites(flipSail);
            }
        }
        else
        {
            if (flipSail != true)
            {
                flipSail = true;
                FlipSprites(flipSail);
            }
        }
        float wind = 1 - Mathf.Abs(Vector2.Dot(windVect.normalized, transform.up.normalized));
        Fill fill = wind > fullSailMin ? Fill.Full : wind > midSailMin ? Fill.Mid : Fill.Low;
        // TODO Adjust volume of sail fill source based on wind.
        SetSprites(fill);
    }

    void SetSprites(Fill fill)
    {
        currentFill = fill;
        switch (fill)
        {
            case Fill.Low:
                darkSail.ChangeSprite(darkSail.low);
                lightSail.ChangeSprite(lightSail.low);
                return;
            case Fill.Mid:
                darkSail.ChangeSprite(darkSail.medium);
                lightSail.ChangeSprite(lightSail.medium);
                return;
            case Fill.Full:
                darkSail.ChangeSprite(darkSail.full);
                lightSail.ChangeSprite(lightSail.full);
                return;
            case Fill.Torn:
                darkSail.ChangeSprite(darkSail.torn);
                lightSail.ChangeSprite(lightSail.torn);
                return;
            case Fill.Closed:
                darkSail.ChangeSprite(darkSail.tied);
                lightSail.ChangeSprite(lightSail.tied);
                return;
        }
    }

    void FlipSprites(bool flipped)
    {
        darkSail.SpriteFlip(flipped);
        lightSail.SpriteFlip(flipped);
    }


    public string GetSailStatus(bool control = false)
    {
        if (control)
        {
            return (open ? "[S] Lower the sail." : "[W] Raise the sail.") + "\n[A & D] Rotate the sail.";
        }
        else
        {
            if (torn)
            {
                return "Sail is torn.";
            }
            else
            {
                switch (currentFill)
                {
                    case Fill.Closed: return "Sail is closed.";
                    case Fill.Full: return "Sail is billowing.";
                    case Fill.Low:
                    case Fill.Mid: return "Sail has slack.";
                    default: return "";
                }
            }
        }
    }
}
