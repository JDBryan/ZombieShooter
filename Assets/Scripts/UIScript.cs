using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIScript : MonoBehaviour
{
    Player player;
    public TMP_Text weaponInfoText;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.activeGun == null) {
            weaponInfoText.SetText("--");
        } else {
            weaponInfoText.SetText(player.activeGun.GetComponent<Pistol>().ammoCount.ToString());
        }
        
    }
}
