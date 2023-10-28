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

    public void SetWaveNumber(int waveNumber) {
        waveNumberText.SetText(waveNumber.ToString());
    }

    public void SetHealthBar(int health) {
        healthBar.SetText(health.ToString());
    }

    public void SetWeaponInfoText(Weapon activeWeapon) {
        string ammoText = activeWeapon.hasInfiniteAmmo ? "inf" : activeWeapon.ammoCount.ToString();
        weaponInfoText.SetText(activeWeapon.name + ": " + ammoText);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Weapon activeWeapon = player.GetActiveWeapon();

        this.SetWeaponInfoText(activeWeapon);

        this.SetHealthBar(player.GetHealth());
    }
}
