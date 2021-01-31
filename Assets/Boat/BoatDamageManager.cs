using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BoatDamageManager : MonoBehaviour
{
    public int hp = 3;
    public int iframes = 0;

    public List<SpriteRenderer> sprites;
    // Initialized outside of class
    private GameMaster gameMaster;

    private AudioSource audioSource;

    public float damageActionProbability = 0.3f;
    List<Action> damageActions;

    // Start is called before the first frame update
    void Start()
    {
        sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        audioSource = GameObject.Find("CrashAudio").GetComponent<AudioSource>();

        // Setup damage actions
        damageActions = new List<Action>();
        damageActions.Add(GetComponentInChildren<Sail>().TearSail);
        damageActions.Add(GetComponentInChildren<Lantern>().Extinguish);
        damageActions.Add(GetComponent<BoatSteering>().RudderBreak);
        damageActions.Add(GetComponent<Captain>().Sleep);
    }

    //// Update is called once per frame
    void Update()
    {
        if (iframes > 0)
        {
            iframes -= 1;
            ShowSprites((iframes % 2) == 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Play crash rock
        audioSource.Play();
        // Disable collision shape to avoid another collision
        collision.collider.enabled = false;
        if (iframes > 0)
        {
            // Add a few more iframes
            iframes += 10;
        }
        else
        {
            // Start flashing
            iframes = 60;
            hp -= 1;
            if (hp < 0)
            {
                ShowSprites(false);
                iframes = 0;
                gameMaster.EndGame(false, GameMaster.endScene.badEnding);
            }
            else
            {
                if (UnityEngine.Random.value < damageActionProbability)
                {
                    // Pick and execute random damage event
                    damageActions[UnityEngine.Random.Range(0, damageActions.Count)].Invoke();
                }
            }
        }
    }
    private void ShowSprites(bool show)
    {
        foreach (var sprite in sprites)
        {
            sprite.enabled = show;
        }
    }
}
