using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    void Start() {
        // Weapon parameters
        this.isAutomatic = false;
        this.fireRate = 1f;
        this.clipSize = 5;
        this.hasInfiniteAmmo = false;
        this.initialAmmoCount = 20;

        // Weapon tracking
        this.currentAmmoCount = this.initialAmmoCount;
        this.roundsLeftInClip = this.clipSize;
        this.lastRoundFiredTime = Time.time;
        this.triggerHeld = false;
        this.roundsFiredWhileTriggerHeld = 0;

        // Sprite rendering
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.userInterface = FindObjectOfType<UserInterface>();
    }

    void LateUpdate() {
        float currentTime = Time.time;
        if ((this.triggerHeld && currentTime - this.lastRoundFiredTime >= this.fireRate)
            && (this.isAutomatic || this.roundsFiredWhileTriggerHeld == 0)) 
        {
            this.Fire();
            this.roundsFiredWhileTriggerHeld += 1;
        }
    }

    public override void PullTrigger()
    {
        this.triggerHeld = true;
    }

    public override void ReleaseTrigger()
    {
        this.triggerHeld = false;
        this.roundsFiredWhileTriggerHeld = 0;
    }
    
    public override void Fire()
    {
        Transform player = this.transform.parent;
        float currentTime = Time.time;
        if (currentTime - this.lastRoundFiredTime >= this.fireRate)
        {
            this.lastRoundFiredTime = currentTime;
            if (this.roundsLeftInClip > 0) {
                this.ChangeSpriteToFire();

                GameObject bullet = Instantiate(bulletPrefab, player.position, player.rotation);
                Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
                Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
                bulletBody.velocity = bulletBody.transform.up * 40;

            if (this.hasInfiniteAmmo){
                this.roundsLeftInClip -= 1;
                userInterface.UpdateWeaponInfo();
            }

            if (!this.hasInfiniteAmmo && this.currentAmmoCount > 0 && this.roundsLeftInClip > 0) {
                this.currentAmmoCount -= 1;
                this.roundsLeftInClip -= 1;
                }
                userInterface.UpdateWeaponInfo();
            }
        }
    }

    public override void ChangeSpriteToFire(){
        gameObject.GetComponent<SpriteRenderer>().sprite = this.fireSprite;
    }

    public override void ChangeSpriteToIdle(){
        gameObject.GetComponent<SpriteRenderer>().sprite = this.idleSprite;
    }

}

