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

    public abstract void Fire(Transform playerTranform);
}
