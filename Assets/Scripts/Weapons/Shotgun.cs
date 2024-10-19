using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int bulletsPerShot;
    
    public override void Fire()
    {
        // Fire round
        this.ChangeSpriteToFire();
        GetComponent<AudioSource>().PlayOneShot(fireSound);
        for (int i = 0; i < this.bulletsPerShot; i++)
        {
            this.CreateProjectile();
        }

        // Update weapon state
        this.roundsLeftInClip -= 1;
        this.roundsFiredWhileTriggerHeld += 1;
        this.lastRoundFiredTime = Time.time;
    }
}

