using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Captain : MonoBehaviour
{
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
    public void Start()
    {
        captainDownClips = Resources.LoadAll<AudioClip>("Sound/captain_needs_tending");
        captainTendedClips = Resources.LoadAll<AudioClip>("Sound/captain_tended");
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
}
