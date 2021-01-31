using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Captain : MonoBehaviour
{
    public bool awake = true;
    public bool onBorad = true;
    [Range(0, 1)]
    public float fallChance = .5f;

    public Sprite noCaptain;
    public SpriteRenderer ship;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Wake()
    {
        awake = true;
    }

    public void Sleep()
    {
        Debug.Log("Captain action");
        if (awake == true)
        {
            awake = false;
        }
        else if(Random.value <= fallChance)
        {
            Fall();
        }
    }

    public void Fall()
    {
        onBorad = false;
        ship.sprite = noCaptain;
    }
}
