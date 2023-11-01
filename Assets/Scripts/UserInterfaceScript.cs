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
    [SerializeField] private GameObject ActiveWeapon;
    [SerializeField] private GameObject SecondaryWeapon;
    [SerializeField] private Sprite PistolSprite;
    [SerializeField] private Sprite ShotgunSprite;
    [SerializeField] private GameObject HealthBarMask;

    public void SetWaveNumber(int waveNumber) {
        waveNumberText.SetText(waveNumber.ToString());
    }

    public void SetHealthBar(int health) {
        if (health >= 0){
            float healthPercent = (float)health / (float)100;
            float newHealthPosition = healthPercent * 7.76f;
            HealthBarMask.transform.SetLocalPositionAndRotation(new Vector3(newHealthPosition, 0f, 0f), HealthBarMask.transform.rotation);
        }
    }

    public void SetWeaponInfoText(Weapon activeWeapon) {
        string ammoText = activeWeapon.hasInfiniteAmmo ? "inf" : activeWeapon.ammoCount.ToString();
        weaponInfoText.SetText(activeWeapon.name + ": " + ammoText);
    }

public void EnableGameOverScreen() {
        Debug.Log("ENDING GAME");
        this.deathScreen.SetActive(true);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Weapon activeWeapon = player.GetActiveWeapon();

        this.SetWeaponInfoText(activeWeapon);
    }
}
