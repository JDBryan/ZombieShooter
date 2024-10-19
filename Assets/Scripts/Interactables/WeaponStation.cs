using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStation : Interactable
{
    private Animator animator;
    private GameObject weapon;
    public GameObject prefabWeapon;
    private GameObject ammoSprite;
    private bool weaponOnSale;
    private bool restockInProgress;
    public int ammoCost;
    private int cost;

    // Start is called before the first frame update
    void Start()
    {
        base.Init();
        this.animator = this.GetComponent<Animator>();
        this.weapon = this.transform.GetChild(1).gameObject;
        this.ammoSprite = this.transform.GetChild(0).gameObject;
        this.weaponOnSale = true;
        this.restockInProgress = false;
        this.cost = baseCost;
        GameController.OnGameReset += ResetStation;
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
        if (this.transform.childCount == 2){
            this.ammoSprite.SetActive(false);
            this.weapon = Instantiate(prefabWeapon, this.transform);
            this.weapon.name = prefabWeapon.name;
            this.weaponOnSale = true;
            this.cost = baseCost;
            this.SetPromptText(this.cost);
        }
    }

    public void ResetStation(){
        this.RestockEnded();
        this.RestockWeapon();
    }

    public override void Interact(){
        if (Player.Instance.money >= this.cost){
            if (this.weaponOnSale){
                Player.Instance.ChangePlayerMoney(-this.cost);
                Player.Instance.PickupWeapon(this.weapon);
                this.weaponOnSale = false;
                this.restockInProgress = true;
                animator.SetBool("Reload", true);
                this.cost = ammoCost;
                this.SetPromptText(this.cost);
            }
            else if (!this.weaponOnSale & !this.restockInProgress & Player.Instance.GetActiveWeapon().gameObject.name == this.prefabWeapon.name){
                Player.Instance.ChangePlayerMoney(-this.cost);
                Player.Instance.RefillAmmoFromStation(this.weapon);
                this.restockInProgress = true;
                animator.SetBool("Reload", true);
                this.ammoSprite.SetActive(false);
            }
            else {
                this.TriggerFailedInteract();
            }
        }
        else {
            this.TriggerFailedInteract();
        }
    }
}
