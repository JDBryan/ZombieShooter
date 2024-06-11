using System;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player parameters
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float inventorySize;
    [SerializeField] private float interactionRadius;

    // Tracking
    [HideInInspector] public int currentHealth;
    [HideInInspector] private int activeWeaponIndex;
    [HideInInspector] private Vector2 velocity;
    [HideInInspector] private List<KnockBack> activeKnockBacks;
    [HideInInspector] public int money;

    // Object references
    [SerializeField] private AudioClip deathNoise;
    [SerializeField] private AudioClip hitNoise;
    [HideInInspector] private Animator animator;
    [HideInInspector] private GameController gameController;
    [HideInInspector] private UserInterface userInterface;
    private Interactable selectedInteractable;

    void Start()
    {
        // Tracking
        this.velocity = new Vector2(0, 0);
        this.activeWeaponIndex = 0;
        this.currentHealth = maxHealth;
        this.activeKnockBacks = new List<KnockBack>();
        this.money = 0;

        // Game object references
        this.gameController = FindObjectOfType<GameController>();
        this.userInterface = FindObjectOfType<UserInterface>();
        this.animator = this.GetComponent<Animator>();
        this.selectedInteractable = null;
    }
    
    void Update()
    {	
        animator.SetFloat("Speed", this.velocity.magnitude);

        if (this.GetActiveWeapon() != null && Time.time - GetActiveWeapon().lastRoundFiredTime > 0.1f){
            this.GetActiveWeapon().ChangeSpriteToIdle();
        }
        FindClosestInteractable();
    }

    void FixedUpdate() {
        UpdateMovement();
        List<KnockBack> finishedKnockbacks = new List<KnockBack>();
        foreach(KnockBack knockback in this.activeKnockBacks) {
            GetComponent<Rigidbody2D>().MovePosition(transform.position + knockback.GetMoveDistance());
            if (knockback.IsOver()) {
                finishedKnockbacks.Add(knockback);
            }
        }   

        foreach(KnockBack knockback in finishedKnockbacks) {
            this.activeKnockBacks.Remove(knockback);
        }
    }

    public void SetVelocity(Vector2 velocity)
    {
        this.velocity = velocity;
    }

    public void PullTrigger() {
        if (this.GetActiveWeapon() != null)
        {
            this.GetActiveWeapon().PullTrigger();
        }
    }

    public void ReleaseTrigger() {
        if (this.GetActiveWeapon() != null)
        {
            this.GetActiveWeapon().ReleaseTrigger();
        }
    }

    public void SwitchWeapon() {
        if (this.GetHeldWeapons().Count < 2) {
            return;
        }
        this.GetActiveWeapon().StopReload();
        this.SetActiveWeapon((activeWeaponIndex + 1) % this.GetHeldWeapons().Count);
    }

    public void ReloadWeapon() 
    {
        if (this.GetActiveWeapon() != null) {
            this.GetActiveWeapon().TriggerReload();
        }
    }

    public void RefillWeaponAmmo(Weapon weapon) {
        weapon.currentAmmoCount = weapon.initialAmmoCount + weapon.roundsLeftInClip;
    }

    void UpdateMovement() 
    {
        float magnitude = this.velocity.magnitude;
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        if (magnitude != 0) {
            float scalar = speed / magnitude;
            Vector2 movement = this.velocity * scalar;
            rigidBody.MovePosition(transform.position + (Vector3)movement);
        }
    }

    // Updates the rotation of the player wrt the current position of the mouse
    public void UpdateRotation(Vector2 mousePosition)
    {
        Vector2 playerPosition = (Vector2)transform.position;
		float angle = AngleBetweenTwoPoints(playerPosition, mousePosition);
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if collision object is a pickup
        if (collision.gameObject.tag == "Pickup")
        {
            this.PickupWeapon(collision.gameObject);
        }

        //Check if collision object is an ammo pack single
        if (collision.gameObject.tag == "AmmoSingleWeapon" && this.GetActiveWeapon() != null)
        {
            RefillWeaponAmmo(GetActiveWeapon());
            userInterface.UpdateWeaponInfo();
            Destroy(collision.gameObject);
        }

        //Check if collision object is an ammo pack all
        if (collision.gameObject.tag == "AmmoAllWeapons")
        {
            foreach (Weapon weapon in GetHeldWeapons()){
                RefillWeaponAmmo(weapon);
            }
            userInterface.UpdateWeaponInfo();
            Destroy(collision.gameObject);
        }

        //Check if collision object is a health pack
        if (collision.gameObject.tag == "Health")
        {   
            this.currentHealth += collision.gameObject.GetComponent<SetHealthPack>().healthAmount;
            if (this.currentHealth > this.maxHealth){
                this.currentHealth = this.maxHealth;
            }
            this.gameController.UpdatePlayerHealth(this.currentHealth);
            Debug.Log(this.currentHealth);
            Destroy(collision.gameObject);
        }

        //Check if collision object is an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            this.Damage(10);
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;
            this.activeKnockBacks.Add(new KnockBack(direction, 0.4f, 0.1f));
        }
    }

    public void Damage(int damageAmount) {
        this.currentHealth -= damageAmount;
        this.gameController.UpdatePlayerHealth(this.currentHealth);
        if (this.currentHealth <= 0){
            this.Kill();
        } else {
            GetComponent<AudioSource>().PlayOneShot(hitNoise);
        }
    }

    public void EnableUserInput()
    {
        this.GetComponent<PlayerUserInput>().enabled = true;
    }

    public void DisableUserInput() {
        this.GetComponent<PlayerUserInput>().enabled = false;
    }

    private void Kill() {
        GetComponent<AudioSource>().PlayOneShot(deathNoise);
        this.animator.SetBool("Dead", true);
        if (this.GetActiveWeapon() != null){
            this.GetActiveWeapon().GetComponent<SpriteRenderer>().enabled = false;
        }
        this.GetComponent<Rigidbody2D>().simulated = false;
        this.GetComponent<PlayerUserInput>().enabled = false;
    }

    public void PickupWeapon(GameObject weapon)
    {
        if (this.GetHeldWeapons().Count >= this.inventorySize) {
            this.DropActiveWeapon();
        }
        weapon.transform.rotation = this.transform.rotation;
        weapon.transform.position = this.transform.position;
        weapon.transform.Rotate(0.0f,0.0f,90.0f);
        weapon.transform.Translate(0.875f,0.0f,0.0f);
        weapon.transform.parent = this.transform;
        weapon.GetComponent<BoxCollider2D>().enabled = false;
        weapon.GetComponent<SpriteRenderer>().enabled = false;
        this.SetActiveWeapon(this.GetHeldWeapons().Count - 1);
    }

    private void DropActiveWeapon()
    {
        Weapon weapon = this.GetActiveWeapon();
        if (weapon == null) {
            return;
        }
        GameObject weaponStation = GameObject.Find("WeaponStation_" + weapon.name);
        if (weaponStation != null){
            weaponStation.GetComponent<WeaponStation>().RestockWeapon();
        }
        weapon.transform.parent = null;
        Destroy(weapon.gameObject);
        this.SetActiveWeapon(this.GetHeldWeapons().Count - 1);
    }

    private void SetActiveWeapon(int index) {
        foreach(Weapon weapon in this.GetHeldWeapons()) {
            weapon.gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
        this.GetHeldWeapons()[index].gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.activeWeaponIndex = index;
        this.userInterface.UpdateWeaponInfo();
    }

    // Helper function to calculate the angle from point a to point b
    float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

    // Returns a list of the currently held weapons 
    public List<Weapon> GetHeldWeapons() {
        List<Weapon> heldWeapons = new List<Weapon>();
        foreach (Transform child in this.transform) {
            if(child.GetComponent<Weapon>() != null) {
                heldWeapons.Add(child.GetComponent<Weapon>());
            }
        }
        return heldWeapons;
    }

    public Weapon GetActiveWeapon()
    {
        if (this.GetHeldWeapons().Count == 0) 
        {
            return null;
        } 
        else
        {
            return this.GetHeldWeapons()[activeWeaponIndex];
        }
    }

    public Weapon GetSecondaryWeapon(){
        if (this.GetHeldWeapons().Count < 2) 
        {
            return null;
        } 
        else
        {
            return this.GetHeldWeapons()[(activeWeaponIndex + 1) % this.GetHeldWeapons().Count];
        }
    }

    public void RefillAmmoFromStation(GameObject stationWeapon){
        foreach (Transform child in this.transform){
            if (child.gameObject == stationWeapon) {
                this.RefillWeaponAmmo(child.GetComponent<Weapon>());
                userInterface.UpdateWeaponInfo();
            }
        }
    }

    public void FindClosestInteractable(){
        Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);
        if (interactables != null & interactables.Length != 0){
            Interactable closestItem = interactables[0];
            float smallestDistance = Mathf.Infinity;

            foreach (Interactable item in interactables){
                float distance = Vector3.Distance(item.gameObject.transform.position, this.transform.position);
                if (distance < smallestDistance){
                    smallestDistance = distance;
                    closestItem = item;
                }
                if (item.selected){
                    item.Deselect();
                }
            }
            this.selectedInteractable = null;
            if (smallestDistance < interactionRadius){
                closestItem.Select();
                this.selectedInteractable = closestItem;
            }
        }
    }

    public void Interact(){
        if (this.selectedInteractable != null){
            this.selectedInteractable.Interact();
        }
    }

    public void ChangePlayerMoney(int changeInMoney){
        this.money += changeInMoney;
        this.userInterface.SetMoneyNumber(this.money);
    }
}