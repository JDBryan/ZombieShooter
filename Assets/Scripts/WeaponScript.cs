using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public string weaponName;
    public GameObject bulletType;
    public float fireRate;
    public float ammoCount;
    public bool hasInfiniteAmmo;
    private SpriteRenderer gunRenderer;
    public Sprite gunFireSprite; 
    public Sprite gunSprite;
    public float lastFireTime;

    public abstract void Fire(Transform playerTranform);

    public abstract void ChangeGunSpriteToFire();

    public abstract void ChangeGunSpriteToIdle();


}
