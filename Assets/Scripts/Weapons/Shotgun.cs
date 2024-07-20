using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    [SerializeField] private int bulletsPerShot;
    
    public override void Fire()
    {
        // Fire round
        Transform player = this.transform.parent;
        this.ChangeSpriteToFire();
        GetComponent<AudioSource>().PlayOneShot(fireSound);
        //Instantiate(shockwavePrefab, this.transform.position, Quaternion.identity, player);
        for (int i = 0; i < this.bulletsPerShot; i++)
        {
            this.CreateProjectile();
        }

        // Update weapon state
        this.roundsLeftInClip -= 1;
        this.roundsFiredWhileTriggerHeld += 1;
        this.lastRoundFiredTime = Time.time;

        // Update user interface
        userInterface.UpdateWeaponInfo();
    }
}

