using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStation : Interactable
{
    private Player player;
    private Animator animator;
    private GameObject weapon;
    public GameObject prefabWeapon;
    private GameObject ammoSprite;
    private bool weaponOnSale;
    private bool restockInProgress;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        this.player = GameObject.FindWithTag("Player").GetComponent<Player>();
        this.animator = this.GetComponent<Animator>();

        this.weapon = this.transform.GetChild(1).gameObject;
        this.ammoSprite = this.transform.GetChild(0).gameObject;
        this.weaponOnSale = true;
        this.restockInProgress = false;
    }

    public void RestockEnded(){
        this.restockInProgress = false;
        animator.SetBool("Reload", false);
        this.ammoSprite.SetActive(true);
        if (this.weaponOnSale == true){
            this.ammoSprite.SetActive(false);
        }
    }

    public void RestockWeapon(){
        this.ammoSprite.SetActive(false);
        this.weapon = Instantiate(prefabWeapon, this.transform);
        this.weapon.name = prefabWeapon.name;
        this.weaponOnSale = true;
    }

    public void ResetStation(Player newPlayer){
        this.player = newPlayer;
        this.RestockEnded();
        this.RestockWeapon();
    }

    public override void Interact(){
        if (this.weaponOnSale == true){
            player.PickupWeapon(this.weapon);
            this.weaponOnSale = false;
            this.restockInProgress = true;
            animator.SetBool("Reload", true);
        }
        else if (this.weaponOnSale == false & this.restockInProgress == false){
            player.RefillAmmoFromStation(this.weapon);
            this.restockInProgress = true;
            animator.SetBool("Reload", true);
            this.ammoSprite.SetActive(false);
        }
    }
}
