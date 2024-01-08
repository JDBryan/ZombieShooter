using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text weaponInfoText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text healthBar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject activeWeaponUI;
    [SerializeField] private GameObject secondaryWeaponUI;
    [SerializeField] private GameObject healthBarMask;
    [SerializeField] private GameObject bulletUI;
    [SerializeField] private GameObject bulletUIParent;
    [SerializeField] private GameObject hud;
    private Vector3 bulletUIPosition;
    public List<GameObject> bulletUIList;

    void Start() {
        this.bulletUIPosition = this.bulletUIParent.transform.position;
        bulletUIList = MakeUIBullets();
        this.UpdateWeaponInfo();
    }

    public void Reset()
    {
        this.player = FindObjectOfType<Player>();
        this.UpdateHealthBar();
        this.UpdateWeaponInfo();
        this.waveNumberText.SetText("");
    }

    public void SetWaveNumber(int waveNumber) {
        waveNumberText.SetText(waveNumber.ToString());
    }

    public void DisableHud() {
        this.hud.SetActive(false);
    }

    public void EnableHud() {
        this.hud.SetActive(true);
    }

    public void UpdateHealthBar() {
        int health = this.player.health;
        if (health >= 0){
            float healthPercent = (float)health / (float)100;
            float newHealthPosition = healthPercent * 7.76f;
            healthBarMask.transform.SetLocalPositionAndRotation(new Vector3(newHealthPosition, 0f, 0f), healthBarMask.transform.rotation);
        }
    }

    private List<GameObject> MakeUIBullets(){
        float pixelSize = 1f / 16f;
        List<GameObject> bulletList = new List<GameObject>();
        for (int index = 0; index < 30; index++){
            float xDistance = (index % 15) * pixelSize * 3f;
            float yDistance = pixelSize * 3f;
            Vector3 bulletPosition = bulletUIPosition + new Vector3(xDistance, 0f, 0f);
            if (index >= 15){
                bulletPosition.y = bulletUIPosition.y - yDistance;
            }
            GameObject newBullet = Instantiate(bulletUI, bulletPosition, this.transform.rotation);
            newBullet.transform.parent = bulletUIParent.transform;
            bulletList.Add(newBullet);         
        }
        return bulletList;
    }

    public void UpdateWeaponInfo() {
        Weapon primaryWeapon = player.GetActiveWeapon();
        Weapon secondaryWeapon = player.GetSecondaryWeapon();
        
        // Update active and non active weapon UI sprites
        this.activeWeaponUI.GetComponent<SpriteRenderer>().sprite = primaryWeapon.gunUISprite;
        this.secondaryWeaponUI.GetComponent<SpriteRenderer>().sprite = secondaryWeapon.gunUISprite;

        // Update clip graphic
        int bulletsInClip = primaryWeapon.clipCount;
        for (int i = 0; i < bulletsInClip; i++) {
            bulletUIList[i].SetActive(true);
        }
        for (int i = bulletsInClip; i < 30; i++) {
            bulletUIList[i].SetActive(false);
        }

        // Update total ammo text
        string ammoText = primaryWeapon.hasInfiniteAmmo ? "inf" : primaryWeapon.ammoCount.ToString();
        weaponInfoText.SetText(ammoText);
    }

    public void SetGameOverScreenActive(bool active) {
        this.deathScreen.SetActive(active);
    }

    public void SetStartMenuActive(bool active) {
        this.startMenu.SetActive(active);
    }
}
