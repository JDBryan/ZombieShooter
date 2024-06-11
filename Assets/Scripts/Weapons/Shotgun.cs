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
        Instantiate(shockwavePrefab, this.transform.position, Quaternion.identity, player);
        for (int i = 0; i < this.bulletsPerShot; i++)
        {
            GameObject bullet = Instantiate(bulletPrefab, player.position, player.rotation);
            bullet.transform.eulerAngles = new Vector3(
                bullet.transform.eulerAngles.x,
                bullet.transform.eulerAngles.y,
                bullet.transform.eulerAngles.z + Random.Range(-this.bulletSpread, this.bulletSpread)
            );
            Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.velocity = bulletBody.transform.up * this.bulletSpeed;
        }

        // Update weapon state
        this.roundsLeftInClip -= 1;
        this.roundsFiredWhileTriggerHeld += 1;
        this.lastRoundFiredTime = Time.time;

        // Update user interface
        userInterface.UpdateWeaponInfo();
    }
}

