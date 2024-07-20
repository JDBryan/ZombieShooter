using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticRifle : Weapon
{
    public override void Fire()
    {
        // Fire round
        this.ChangeSpriteToFire();
        GetComponent<AudioSource>().PlayOneShot(fireSound);
        this.CreateProjectile();

        // Update weapon state 
        this.lastRoundFiredTime = Time.time;
        this.roundsFiredWhileTriggerHeld += 1;
        this.roundsLeftInClip -= 1;

        // Update user interface
        userInterface.UpdateWeaponInfo();
    }
}