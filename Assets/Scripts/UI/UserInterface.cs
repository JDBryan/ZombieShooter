using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UserInterface : MonoBehaviour
{
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
        this.DisableHud();

        GameController.OnWaveStarted += SetWaveNumber;
        GameController.OnGameReset += OnGameReset;
        GameController.OnGameStart += OnGameStart;
        GameController.OnGamePaused += OnGamePaused;
        GameController.OnGameResumed += OnGameResumed;
        Player.OnHealthTotalChanged += UpdateHealthBar;
        Player.OnKilled += DisableHud;
        Player.OnActiveWeaponChanged += UpdateWeaponInfo;
        Weapon.OnAmmoCountChanged += UpdateAmmoCount;
        Weapon.OnRoundsInClipChanged += UpdateClipUI;

    }

    public void OnGameReset()
    {
        this.SetPauseMenuActive(false);
        this.SetGameOverScreenActive(false);
        this.SetStartMenuActive(true);
    }

    public void OnGameStart()
    {
        this.EnableHud();
        this.UpdateWeaponInfo(null, null);
        this.waveNumberText.SetText("");
        this.SetMoneyNumber(0);
        this.SetStartMenuActive(false);
    }

    public void OnGamePaused()
    {
        this.SetPauseMenuActive(true);
        this.DisableHud();
    }

    public void OnGameResumed()
    {
        this.SetPauseMenuActive(false);
        this.EnableHud();
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

    public void UpdateHealthBar(int healthTotal) {
        if (healthTotal >= 0){
            float healthPercent = (float)healthTotal / (float)100;
            Material material = healthBar.GetComponent<SpriteRenderer>().material;
            material.SetFloat("_Progress", healthPercent);
        }
    }

    public void UpdateAmmoCount(Weapon weapon) {
        if (weapon.isActive)
        {
            string ammoText = weapon.hasInfiniteAmmo ? "inf" : weapon.currentAmmoCount.ToString();
            weaponInfoText.SetText(ammoText);
        }
    }

    public void UpdateClipUI(Weapon weapon) {
        if (weapon.isActive)
        {
            for (int clip = 0; clip < clips.transform.childCount; clip++)
            {
                Transform child = clips.transform.GetChild(clip);
                if (child.gameObject.GetComponent<Clip>().weapon.name == weapon.name)
                {
                    child.gameObject.SetActive(true);
                    child.gameObject.GetComponent<Clip>().UpdateClipInfo(weapon);
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }
    }

    public void UpdateWeaponInfo(Weapon primaryWeapon, Weapon secondaryWeapon) {
        if (primaryWeapon == null) 
        {
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

        this.UpdateClipUI(primaryWeapon);
        this.UpdateAmmoCount(primaryWeapon);
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
