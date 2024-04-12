using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    int bulletsPerShot;
    void Start() {
        // Weapon parameters
        this.isAutomatic = false;
        this.fireRate = 0.5f;
        this.clipSize = 5;
        this.hasInfiniteAmmo = false;
        this.initialAmmoCount = 20;
        this.bulletsPerShot = 5;

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
                GetComponent<AudioSource>().PlayOneShot(fireSound);
                for (int i = 0; i < this.bulletsPerShot; i++)
                {
                    GameObject bullet = Instantiate(bulletPrefab, player.position, player.rotation);
                    bullet.transform.eulerAngles = new Vector3(
                        bullet.transform.eulerAngles.x,
                        bullet.transform.eulerAngles.y,
                        bullet.transform.eulerAngles.z + Random.Range(-10, 10)
                    );
                    Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
                    Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
                    bulletBody.velocity = bulletBody.transform.up * 40;
                }

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

