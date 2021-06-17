using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Captain : MonoBehaviour
{
    [HideInInspector] public Boat boat;
    [HideInInspector] public Transform lighthouse;
    public bool awake = true;
    public bool onBorad = true;
    [Range(0, 1)]
    public float fallChance = .5f;

    // Initialized in inspector
    public Sprite noCaptain;
    public SpriteRenderer ship;
    public string captainDownClipDirectory;
    public string captainTendedClipDirectory;

    // Initialized via BindBoatToBoatWidget()
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public Image img;
    [HideInInspector]
    public BoatWidget boatWidget;

    public Sprite awakeSpr;
    public Sprite sleepingSpr;

    new public AudioSource audio;

    private AudioClip[] captainDownClips;
    private AudioClip[] captainTendedClips;
    private int tic1;
    private int tic2;

    public float directionCalloutFrequency;
    private WaitForSeconds directionCalloutWait;
    private Vector2 lastCalloutKey;

    private Dictionary<Vector2, DialogueManager.Messages> directionMap;
    public void Start()
    {
        captainDownClips = Resources.LoadAll<AudioClip>("Sound/captain_needs_tending");
        captainTendedClips = Resources.LoadAll<AudioClip>("Sound/captain_tended");
        directionMap = new Dictionary<Vector2, DialogueManager.Messages>()
        {
            {Vector2.down , DialogueManager.Messages.directionCallSouth},
            {Vector2.up, DialogueManager.Messages.directionCallNorth},
            {Vector2.left, DialogueManager.Messages.directionCallWest},
            {Vector2.right, DialogueManager.Messages.directionCallEast},
            {new Vector2(0.785398f,0.785398f), DialogueManager.Messages.directionCallNorthEast},
            {new Vector2(0.785398f,-0.785398f), DialogueManager.Messages.directionCallSouthEast},
            {new Vector2(-0.785398f,0.785398f), DialogueManager.Messages.directionCallNorthWest},
            {new Vector2(-0.785398f,-0.785398f), DialogueManager.Messages.directionCallSouthWest}
        };
        directionCalloutWait = new WaitForSeconds(directionCalloutFrequency);
        StartCoroutine(nameof(DirectionCallout));
    }

    IEnumerator DirectionCallout()
    {
        while (true)
        {
            if (awake && onBorad)
            {
                var maxDot = -1f;
                var keyOfMaxDot = Vector2.down;
                var normal = ( lighthouse.position - boat.transform.position).normalized;
                // Check if player is going totally wrong way
                if (Vector3.Dot(boat.transform.up, normal) < -0.5f)
                {
                    
                    DialogueManager.singleton.DisplayMessage(DialogueManager.Messages.wrongDirection);
                    goto EndOfOutmostIf;
                }
                foreach (var directionMapKey in directionMap.Keys)
                {
                    var dot = Vector3.Dot(directionMapKey, normal);
                    if (dot > maxDot)
                    {
                        keyOfMaxDot = directionMapKey;
                        maxDot = dot;
                    }
                }
                if (keyOfMaxDot != lastCalloutKey) DialogueManager.singleton.DisplayMessage(directionMap[keyOfMaxDot]);
                lastCalloutKey = keyOfMaxDot;
            }
            EndOfOutmostIf:
            yield return directionCalloutWait;
        }
    }

    public void Wake()
    {
        awake = true;
        img.sprite = awakeSpr;
        IncrementTic(ref tic1, captainTendedClips);
        audio.PlayOneShot(captainTendedClips[tic1]);
    }

    public void Sleep()
    {
        Sleep(false);
    }
    public void Sleep(bool silently)
    {
        //Debug.Log("Captain action");
        if (awake == true)
        {
            awake = false;
            if (silently == false)
            {
                IncrementTic(ref tic2, captainTendedClips);
                audio.PlayOneShot(captainDownClips[tic2]);
            }
            img.sprite = sleepingSpr;
        }
        else if (Random.value <= fallChance)
        {
            Fall();
        }
    }

    public void Fall()
    {
        if (!awake)
        {
            onBorad = false;
            ship.sprite = noCaptain;
            anim.Play("CaptainSlide");
            boatWidget.stations[(int)BoatWidget.StationNames.tend].HideSelf(totally: true);
        }
    }

    private void IncrementTic(ref int tic, object[] audioSources)
    {
        tic += 1;
        if (tic >= audioSources.Length)
        {
            tic = 0;
        }
    }

    public string GetCaptainStatus(bool b = false)
    {
        if (awake)
        {
        return "The captain is conscious.";
            
        }
        else
        {
            if (onBorad)
            {
                return "The captain is unconscious.";
            }
            else
            {
                return "The captain has fallen overboard.";
            }
        }
    }
}
