using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private TMP_Text weaponInfoText;
    [SerializeField] private TMP_Text waveNumberText;
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject deathScreen;
    [SerializeField] private GameObject startMenu;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject activeWeaponUI;
    [SerializeField] private GameObject secondaryWeaponUI;
    [SerializeField] private GameObject hud;
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private GameObject reloadBar;
    [SerializeField] private GameObject clips;
    [SerializeField] private GameObject weaponUI;

    private Vector3 gunHUDPosition;

    void Start() {
        this.UpdateWeaponInfo();
        this.DisableHud();
        this.SetMoneyNumber(0);
    }

    public void Reset()
    {
        this.player = FindObjectOfType<Player>();
        this.UpdateHealthBar();
        this.UpdateWeaponInfo();
        this.waveNumberText.SetText("");
        this.moneyText.SetText("");
    }

    public void PlayButtonSelectSound() {
        GetComponent<AudioSource>().PlayOneShot(buttonHoverSound);
    }

    public void PlayButtonClickSound() {
        GetComponent<AudioSource>().PlayOneShot(buttonClickSound);
    }

    public void SetWaveNumber(int waveNumber) {
        waveNumberText.SetText(waveNumber.ToString());
    }

    public void SetReloadBar(float progress)
    {
        reloadBar.SetActive(true);
        reloadBar.GetComponent<ReloadBar>().progress = progress;
        if (progress >= 1f)
        {
            reloadBar.SetActive(false);
        }
    }

    public void SetMoneyNumber(int money){
        moneyText.SetText(money.ToString());
    }

    public void DisableHud() {
        this.hud.SetActive(false);
    }

    public void EnableHud() {
        this.hud.SetActive(true);
    }

    public void UpdateHealthBar() {
        int health = this.player.currentHealth;
        if (health >= 0){
            float healthPercent = (float)health / (float)100;
            Material material = healthBar.GetComponent<SpriteRenderer>().material;
            material.SetFloat("_Progress", healthPercent);
        }
    }

    public void UpdateWeaponInfo() {
        Weapon primaryWeapon = player.GetActiveWeapon();
        Weapon secondaryWeapon = player.GetSecondaryWeapon();

        if (primaryWeapon == null) {
            this.weaponUI.SetActive(false);
            return;
        } 
        else 
        {
            this.weaponUI.SetActive(true);
        }
        
        // Update active and non active weapon UI sprites
        this.activeWeaponUI.GetComponent<SpriteRenderer>().sprite = primaryWeapon == null ? null : primaryWeapon.userInterfaceSprite;
        this.secondaryWeaponUI.GetComponent<SpriteRenderer>().sprite = secondaryWeapon == null ? null : secondaryWeapon.userInterfaceSprite;


        // Update clip graphic
        for (int clip = 0; clip < clips.transform.childCount; clip++){
            Transform child = clips.transform.GetChild(clip);
            if (child.gameObject.GetComponent<Clip>().weapon.name == primaryWeapon.name){
                child.gameObject.SetActive(true);
                child.gameObject.GetComponent<Clip>().UpdateClipInfo(primaryWeapon);
            }
            else{
                child.gameObject.SetActive(false);
            }
        }

        // Update total ammo text
        string ammoText = primaryWeapon.hasInfiniteAmmo ? "inf" : primaryWeapon.currentAmmoCount.ToString();
        weaponInfoText.SetText(ammoText);
    }

    public void SetGameOverScreenActive(bool active) {
        this.deathScreen.SetActive(active);
    }

    public void SetStartMenuActive(bool active) {
        this.startMenu.SetActive(active);
    }

    public void SetPauseMenuActive(bool active) {
        this.pauseMenu.SetActive(active);
    }
}
