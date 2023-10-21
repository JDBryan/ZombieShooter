using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameController gameController;
    [SerializeField] private TMP_Text weaponInfoText;
    [SerializeField] private TMP_Text roundNumberText;

    // Update is called once per frame
    void LateUpdate()
    {
        Weapon activeWeapon = player.GetActiveWeapon();
        string ammoCount = "inf";
        if (!activeWeapon.hasInfiniteAmmo) {
            ammoCount = activeWeapon.ammoCount.ToString();
        }
        weaponInfoText.SetText(player.GetActiveWeapon().name + ": " + ammoCount);

        roundNumberText.SetText(gameController.GetWaveNumber().ToString());
    }
}
