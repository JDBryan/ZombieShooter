using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{

    void Start() {
        this.weaponName = "Shotgun";
        this.hasInfiniteAmmo = false;
        this.ammoCount = 20;
    }
    
    public override void Fire(Transform player)
    {
        if (this.hasInfiniteAmmo || this.ammoCount > 0) {
            GameObject bullet = Instantiate(bulletType, player.position, player.rotation);
            Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(bulletBody.transform.up * 4000);
        }
        if (!this.hasInfiniteAmmo && this.ammoCount > 0) {
            this.ammoCount -= 1;
        }
    }

    public override void ChangeGunSpriteToFire(){
        gameObject.GetComponent<SpriteRenderer>().sprite = gunFireSprite;
    }

    public override void ChangeGunSpriteToIdle(){
        gameObject.GetComponent<SpriteRenderer>().sprite = gunSprite;
    }

}

