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
    [SerializeField] public int damage;

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

    public void CreateProjectile()
    {
        Transform player = this.transform.parent;
        GameObject bullet = Instantiate(bulletPrefab, player.position + Vector3.Normalize(player.transform.rotation * new Vector3(0f,1f,0f))*1.5f, player.rotation);
        bullet.transform.eulerAngles = new Vector3(
                bullet.transform.eulerAngles.x,
                bullet.transform.eulerAngles.y,
                bullet.transform.eulerAngles.z + Random.Range(-this.bulletSpread, this.bulletSpread)
            );
        bullet.GetComponent<Bullet>().damage = this.damage;
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), player.GetComponent<CircleCollider2D>());
        Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
        bulletBody.velocity = bulletBody.transform.up * this.bulletSpeed;
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
