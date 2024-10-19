using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUserInput : MonoBehaviour
{
    void Update()
    {
        Player.Instance.SetVelocity(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")));

        if (Input.GetKeyDown("e")) {
            Player.Instance.SwitchWeapon();
        }

        if (Input.GetKeyDown("r")) {
            Player.Instance.ReloadWeapon();
        }

        if (Input.GetMouseButtonDown(0)) {
            Player.Instance.PullTrigger();
        }

        if (Input.GetMouseButtonUp(0)) {
            Player.Instance.ReleaseTrigger();
        }

        if (Input.GetKeyDown("f")) {
            Player.Instance.Interact();
        }
    }

    void LateUpdate() 
    {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Player.Instance.UpdateRotation(mousePosition);
    }
}