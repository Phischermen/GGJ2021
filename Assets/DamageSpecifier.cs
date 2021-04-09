using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageSpecifier : MonoBehaviour
{
    public enum DamageSelectType
    {
        invokeAllSelectedEvents = 0, // 00
        invokeRandomSelectedEvent = 1, // 01
        invokeAllSelectedEventOnCoinFlip = 2, // 10
        invokeRandomSelectedEventOnCoinFlip = 3, // 11
    }
    [System.Flags]
    public enum ShipDamageEvent
    {
        ripSail = 1 << 0,
        breakRudder = 1 << 1,
        knockOutCaptain = 1 << 2,
        blowOutLantern = 1 << 3,

    }
    public int damageToDeal = 1;
    public int iframesToDealOnHit = 60;
    public int iframesToDealOnConsecutiveHit = 10;
    public DamageSelectType damageSelectType;
    public ShipDamageEvent shipDamageEvent;
    public AudioClip crashSFX;
}
