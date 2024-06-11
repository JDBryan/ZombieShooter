using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // Weapon parameters
    [SerializeField] public GameObject bulletPrefab;
    [SerializeField] public bool isAutomatic;
    [SerializeField] public float fireRate;
    [SerializeField] public int clipSize; // Maximum number of rounds that can be held in the clip
    [SerializeField] public bool hasInfiniteAmmo;
    [SerializeField] public int initialAmmoCount; // Number of rounds the player starts with
    [SerializeField] public AudioClip fireSound;
    [SerializeField] public float reloadTime;
    [SerializeField] public float bulletSpeed;
    [SerializeField] public float bulletSpread;

    // Sprite rendering
    [SerializeField] public Sprite fireSprite; 
    [SerializeField] public Sprite idleSprite;
    [SerializeField] public Sprite userInterfaceSprite;
    [SerializeField] public GameObject shockwavePrefab;
    [HideInInspector] public UserInterface userInterface;
    [HideInInspector] public SpriteRenderer spriteRenderer;

    // Weapon state
    [HideInInspector] public float reloadStartTime;
    [HideInInspector] public bool reloadInProgress;
    [HideInInspector] public int currentAmmoCount; // This value does not include rounds in the clip
    [HideInInspector] public int roundsLeftInClip;
    [HideInInspector] public float lastRoundFiredTime;
    [HideInInspector] public bool triggerHeld;
    [HideInInspector] public int roundsFiredWhileTriggerHeld;

    public void Start()
    {
        this.currentAmmoCount = this.initialAmmoCount;
        this.roundsLeftInClip = this.clipSize;
        this.lastRoundFiredTime = Time.time;
        this.triggerHeld = false;
        this.roundsFiredWhileTriggerHeld = 0;
        this.spriteRenderer = this.GetComponent<SpriteRenderer>();
        this.userInterface = FindObjectOfType<UserInterface>();
    }

    public void LateUpdate()
    {
        // Check if a round should be fired
        float currentTime = Time.time;
        if (this.triggerHeld && currentTime - this.lastRoundFiredTime >= this.fireRate
            && (this.isAutomatic || this.roundsFiredWhileTriggerHeld == 0)) 
        {
            if (this.roundsLeftInClip > 0)
            {
                this.Fire();
                this.StopReload();
            }
            else
            {
                this.TriggerReload();
            }
        }

        // Handle in progress reloads
        if (this.reloadInProgress)
        {
            float timeSinceReloadStart = currentTime - this.reloadStartTime;
            userInterface.SetReloadBar(timeSinceReloadStart / this.reloadTime);
            if (timeSinceReloadStart >= this.reloadTime)
            {
                int emptyClipSpace = this.clipSize - this.roundsLeftInClip;
                int roundsToReload = this.currentAmmoCount >= emptyClipSpace ? 
                    emptyClipSpace : this.currentAmmoCount;
                this.currentAmmoCount -= roundsToReload;
                this.roundsLeftInClip += roundsToReload;
                this.reloadInProgress = false;
                userInterface.UpdateWeaponInfo();
            }
        }
    }

    // Fires a single round from the gun
    public virtual void Fire()
    {
        return;
    }

    public void PullTrigger()
    {
        this.triggerHeld = true;
    }

    public void ReleaseTrigger()
    {
        this.triggerHeld = false;
        this.roundsFiredWhileTriggerHeld = 0;
    }

    public void ChangeSpriteToFire()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = this.fireSprite;
    }

    public void ChangeSpriteToIdle()
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = this.idleSprite;
    }

    public void TriggerReload()
    {
        if (!this.reloadInProgress && this.roundsLeftInClip < this.clipSize && this.currentAmmoCount > 0) {
            this.reloadStartTime = Time.time;
            this.reloadInProgress = true;
        }
    }

    public void StopReload()
    {
        this.reloadInProgress = false;
        this.userInterface.SetReloadBar(1);
    }   
}
