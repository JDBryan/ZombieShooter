using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public GameObject bulletType;
    public float fireRate;
    public float ammoCount;

    public abstract void Fire();
}
