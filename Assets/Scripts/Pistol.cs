using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour
{
    public GameObject bulletType;
    public float fireRate;
    private float ammoCount = 20;

    public void Fire()
    {
        if (ammoCount != 0) {
            Transform player = transform.parent.transform;
            GameObject bullet = Instantiate(bulletType, player.position, player.rotation);
            Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
            Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
            bulletBody.AddForce(bulletBody.transform.up * 4000);
            ammoCount -= 1;
        } else {
            Debug.Log("Click");
        }
    }
}

