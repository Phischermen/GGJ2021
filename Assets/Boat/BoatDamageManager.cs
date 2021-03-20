using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BoatDamageManager : MonoBehaviour
{
    public enum DamageActions
    {
        sail,
        rudder,
        captain,
        lantern,
        numberOfDamageActions
    }
    public int hp = 3;
    public DamageActions damageActionTest;
    int maxHp;
    [HideInInspector]
    public int iframes = 0;
    public int iframesOnHit = 60;
    public int iframesOnConsecutiveHit = 10;

    public List<SpriteRenderer> sprites;
    // Initialized outside of class
    public GameMaster gameMaster;
    public BoatWidget boatWidget;
    public Image lifeRing;

    private AudioSource audioSource;

    public float damageActionProbability = 0.3f;
    List<Action> damageActions;

    // Start is called before the first frame update
    void Start()
    {
        maxHp = hp;
        sprites = new List<SpriteRenderer>(GetComponentsInChildren<SpriteRenderer>());
        //gameMaster = GameObject.Find("GameMaster").GetComponent<GameMaster>();
        //gameMaster = GameObject.FindObjectOfType<GameMaster>();
        audioSource = GameObject.Find("CrashAudio").GetComponent<AudioSource>();
    }
    public void SetupDamageActions()
    {
        // Setup damage actions
        damageActions = new List<Action>();
        for (int i = 0; i < (int)DamageActions.numberOfDamageActions; i++)
        {
            // Initialize with empty actions
            damageActions.Add(()=>{});
        }
        damageActions[(int)DamageActions.sail] += GetComponentInChildren<Sail>().TearSail;
        damageActions[(int)DamageActions.sail] += boatWidget.SailTorn;
        damageActions[(int)DamageActions.lantern] += GetComponentInChildren<Lantern>().Extinguish;
        damageActions[(int)DamageActions.lantern] += boatWidget.LanternExtinguished;
        damageActions[(int)DamageActions.rudder] += GetComponent<BoatSteering>().RudderBreak;
        damageActions[(int)DamageActions.rudder] += boatWidget.RudderBroke;
        damageActions[(int)DamageActions.captain] += GetComponent<Captain>().Sleep;
        damageActions[(int)DamageActions.captain] += boatWidget.CaptainAsleep;
    }

    //// Update is called once per frame
    void Update()
    {
        if (iframes > 0)
        {
            iframes -= 1;
            ShowSprites((iframes % 2) == 0);
        }
        lifeRing.fillAmount = Mathf.Lerp(lifeRing.fillAmount, (float)hp / (float)maxHp, 0.1f);
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
            iframes += iframesOnConsecutiveHit;
        }
        else if (iframes == 0)
        {
            // Start flashing
            iframes = iframesOnHit;
            hp -= 1;
            if (hp <= 0)
            {
                ShowSprites(false);
                iframes = 0;
                GameMaster.endScene end = UnityEngine.Random.value > .5 ? GameMaster.endScene.badEnding : GameMaster.endScene.badEnding2;
                gameMaster.EndGame(false, GameMaster.endScene.badEnding);
            }
            else
            {
                if (damageActionTest == DamageActions.numberOfDamageActions)
                {
                    if (UnityEngine.Random.value < damageActionProbability)
                    {
                        // Pick and execute random damage event
                        damageActions[UnityEngine.Random.Range(0, damageActions.Count)].Invoke();
                    }
                }
                else
                {
                    // Play specific damage event for testing
                    damageActions[(int)damageActionTest].Invoke();
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
