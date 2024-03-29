using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Weapon parameters
    public GameObject bulletPrefab;
    public bool isAutomatic;
    public float fireRate;
    public int clipSize;
    public bool hasInfiniteAmmo;
    public int initialAmmoCount;

    // Weapon tracking
    public int currentAmmoCount;
    public int roundsLeftInClip;
    public float lastRoundFiredTime;
    public bool triggerHeld;
    public int roundsFiredWhileTriggerHeld;

    // Sprite rendering
    public SpriteRenderer spriteRenderer;
    public Sprite fireSprite; 
    public Sprite idleSprite;
    public Sprite userInterfaceSprite;
    public UserInterface userInterface;

    // Fires a single round from the gun
    public abstract void Fire();

    // Sets trigger state to held
    public abstract void PullTrigger();

    // Sets trigger state to not held
    public abstract void ReleaseTrigger();

    // Sets the weapon sprite to the firing sprite
    public abstract void ChangeSpriteToFire();

    // Sets the weapon sprite to the idle sprite
    public abstract void ChangeSpriteToIdle();


}
