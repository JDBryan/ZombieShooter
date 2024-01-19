using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUserInput : MonoBehaviour
{
    private Player player;

    void Start()
    {
        this.player = this.GetComponent<Player>();
    }

    void Update()
    {
        player.SetVelocity(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetKeyDown("e")) {
            player.SwitchWeapon();
        }

        if (Input.GetKeyDown("r")) {
            player.ReloadWeapon();
        }

        if (Input.GetMouseButtonDown(0)) {
            player.PullTrigger();
        }

        if (Input.GetMouseButtonUp(0)) {
            player.ReleaseTrigger();
        }
    }

    void LateUpdate() 
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        player.UpdateRotation(mousePosition);
    }
}