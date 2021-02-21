using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sail : MonoBehaviour
{
    BoatSteering boat;
    public bool open = true;
    public bool torn = false;
    public bool atSail = false;
    public float turnSpeed = 5;
    public SailSprite lightSail;
    public SailSprite darkSail;

    [Range(0, 1)]
    public float midSailMin = .33f;
    [Range(0, 1)]
    public float fullSailMin = .66f;

    [SerializeField]
    private bool flipSail = false;

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
        //sailBreakAudioSource = GameObject.Find("SailAudio").GetComponent<AudioSource>();
        boat = transform.parent.GetComponent<BoatSteering>();
        boat.sail = this;
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
            }
            float rotation = Input.GetAxis("Horizontal");
            if (Mathf.Abs(rotation) > .1)
            {
                transform.Rotate(Vector3.forward * rotation * turnSpeed * Time.deltaTime);
            }
        }
    }

    public void OpenClose()
    {
        Fill fill = open ? Fill.Closed : Fill.Low;
        SetSprites(fill);
        open = !open;
    }

    public void OpenClose(bool opening)
    {
        Fill fill = opening ? Fill.Low : Fill.Closed;
        SetSprites(fill);
        open = opening;
        if (opening)
        {
            sailBreakAudioSource.PlayOneShot(sailRaise);
        }
        else
        {
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

    enum Fill
    {
        Low,
        Mid,
        Full,
        Closed,
        Torn, // TODO Add sprites for torn sail
    }
}
