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

    // Update is called once per frame
    void LateUpdate()
    {
        Weapon activeWeapon = player.GetActiveWeapon();
        string ammoCount = "inf";
        if (!activeWeapon.hasInfiniteAmmo) {
            ammoCount = activeWeapon.ammoCount.ToString();
        }
        weaponInfoText.SetText(player.GetActiveWeapon().name + ": " + ammoCount);

        healthBar.SetText(player.GetHealth().ToString());
    }
}
