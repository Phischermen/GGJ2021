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
    //public DamageActions damageActionTest;
    int maxHp;
    public float reboundStrength = 0.5f;
    public GameObject objectDestroyPrefab;
    [HideInInspector]
    public int iframes = 0;
    //public int iframesOnHit = 60;
    //public int iframesOnConsecutiveHit = 10;

    public List<SpriteRenderer> sprites;
    // Initialized outside of class
    public GameMaster gameMaster;
    public BoatWidget boatWidget;
    public Boat boat;
    public GameObject flag;
    public Image lifeRing;

    private AudioSource audioSource;
    private Animator animator;

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
        animator = GetComponent<Animator>();
    }
    public void SetupDamageActions()
    {
        // Setup damage actions
        damageActions = new List<Action>();
        for (int i = 0; i < (int)DamageActions.numberOfDamageActions; i++)
        {
            // Initialize with empty actions
            damageActions.Add(() => { });
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
        if (hp <= 0) return;
        var ds = collision.gameObject.GetComponentInParent<DamageSpecifier>();
        if (ds)
        {
            // Play crash sfx
            audioSource.PlayOneShot(ds.crashSFX);
            if (ds.rebounder)
            {
                // Bounce the boat away from obstacle.
                Vector2 normal = Vector2.zero;
                foreach (var contact in collision.contacts)
                {
                    normal += contact.normal;
                }
                normal = normal.normalized;
                boat.steering.moveVector += normal * reboundStrength;
            }
            else
            {
                // Destroy damage specifier object to avoid another collision
                Destroy(ds.gameObject);
                Instantiate(objectDestroyPrefab, collision.transform.position, Quaternion.identity);
            }
            // Check if damage has been dealt to avoid dealing damage again.
            if (ds.damageDealt == false)
            {
                ds.damageDealt = true;
                if (iframes > 0)
                {
                    // Add a few more iframes
                    iframes += ds.iframesToDealOnConsecutiveHit;
                }
                else if (iframes == 0)
                {
                    // Start flashing
                    iframes = ds.iframesToDealOnHit;
                    hp -= ds.damageToDeal;
                    if (hp <= 0)
                    {
                        ShowSprites(true);
                        iframes = 0;
                        animator.SetBool("sink", true);
                        flag.SetActive(false);
                    }
                    else
                    {
                        // Check bits and determine the damage select process to be executed
                        var random = ((int)ds.damageSelectType & 0b01) == 0b01;
                        var coinflip = ((int)ds.damageSelectType & 0b10) == 0b10;
                        if (coinflip)
                        {
                            if (UnityEngine.Random.value > 0.5f)
                            {
                                // Coin flip passed. Invoke event(s).
                                PickEvent(ds, random);
                            }
                            else
                            {
                                // Coin flip did not pass. Don't invoke anything.
                            }
                        }
                        else
                        {
                            PickEvent(ds, random);
                        }
                    }
                }
            }
        }

    }


    private void ShowSprites(bool show)
    {
        // If we are not playing the floating animation, then we don't want to effect sprites visibility
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("floating") == false) return;
        foreach (var sprite in sprites)
        {
            sprite.enabled = show;
        }
    }


    private void PickEvent(DamageSpecifier ds, bool random)
    {
        List<DamageActions> vs = new List<DamageActions>();
        if ((ds.shipDamageEvent & DamageSpecifier.ShipDamageEvent.ripSail) == DamageSpecifier.ShipDamageEvent.ripSail) vs.Add(DamageActions.sail);
        if ((ds.shipDamageEvent & DamageSpecifier.ShipDamageEvent.breakRudder) == DamageSpecifier.ShipDamageEvent.breakRudder) vs.Add(DamageActions.rudder);
        if ((ds.shipDamageEvent & DamageSpecifier.ShipDamageEvent.knockOutCaptain) == DamageSpecifier.ShipDamageEvent.knockOutCaptain) vs.Add(DamageActions.captain);
        if ((ds.shipDamageEvent & DamageSpecifier.ShipDamageEvent.blowOutLantern) == DamageSpecifier.ShipDamageEvent.blowOutLantern) vs.Add(DamageActions.lantern);
        if (random)
        {
            var choice = (int)UnityEngine.Random.Range(0f, vs.Count - 0.01f);
            damageActions[choice].Invoke();
        }
        else
        {
            foreach (var choice in vs)
            {
                damageActions[(int)choice].Invoke();
            }
        }
    }


    private void GoToGameOverScreen()
    {
        //var end = UnityEngine.Random.value > .5 ? GameMaster.endScene.badEnding : GameMaster.endScene.badEnding2;
        gameMaster.EndGame(false, GameMaster.endScene.badEnding);
    }
}
