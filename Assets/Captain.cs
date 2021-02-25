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

    public Sprite noCaptain;
    public SpriteRenderer ship;

    public Animator anim;
    public Image img;
    public Sprite awakeSpr;
    public Sprite sleepingSpr;

    public void Wake()
    {
        awake = true;
        img.sprite = awakeSpr;
    }

    public void Sleep()
    {
        Debug.Log("Captain action");
        if (awake == true)
        {
            awake = false;
            img.sprite = sleepingSpr;
        }
        else if(Random.value <= fallChance)
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
        }
    }
}
