using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // Player parameters
    private int maxHealth;
    private float speed;

    // Tracking
    public int health;
    private int activeWeaponIndex;
    private Vector2 velocity;
    private List<KnockBack> activeKnockBacks;

    // Game object references
    [SerializeField] private Animator animator;
    private GameController gameController;
    private UserInterface userInterface;

    void Start()
    {
        // Player parameters
        this.maxHealth = 100;
        this.speed = 0.14f;

        // Tracking
        this.velocity = new Vector2(0, 0);
        this.activeWeaponIndex = 0;
        this.health = maxHealth;
        this.activeKnockBacks = new List<KnockBack>();

        // Game object references
        this.gameController = FindObjectOfType<GameController>();
        this.userInterface = FindObjectOfType<UserInterface>();
        
        this.GetComponent<Rigidbody2D>().MovePosition(new Vector3(0, 65, 0));
    }
    
    void Update()
    {	
        animator.SetFloat("Speed", this.velocity.magnitude);

        if (Time.time - GetActiveWeapon().lastRoundFiredTime > 0.1f){
            this.GetActiveWeapon().ChangeSpriteToIdle();
        }
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
        this.GetActiveWeapon().PullTrigger();
    }

    public void ReleaseTrigger() {
        this.GetActiveWeapon().ReleaseTrigger();
    }

    public void FireActiveWeapon() {
        this.GetActiveWeapon().Fire();
        userInterface.UpdateWeaponInfo();
    }

    public void SwitchWeapon() {
        this.GetActiveWeapon().gameObject.GetComponent<SpriteRenderer>().enabled = false;
        this.GetSecondaryWeapon().gameObject.GetComponent<SpriteRenderer>().enabled = true;
        this.activeWeaponIndex = (activeWeaponIndex + 1) % this.GetHeldWeapons().Count;
        this.userInterface.UpdateWeaponInfo();
    }

    public void ReloadWeapon() 
    {
        if (this.GetActiveWeapon().currentAmmoCount >= this.GetActiveWeapon().clipSize 
            || this.GetActiveWeapon().hasInfiniteAmmo){
            this.GetActiveWeapon().roundsLeftInClip = this.GetActiveWeapon().clipSize;
        } else {
            this.GetActiveWeapon().roundsLeftInClip = this.GetActiveWeapon().currentAmmoCount;
        }

        this.userInterface.UpdateWeaponInfo();
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
            GameObject weapon = collision.gameObject;
            weapon.transform.rotation = this.transform.rotation;
            weapon.transform.position = this.transform.position;
            weapon.transform.Rotate(0.0f,0.0f,90.0f);
            weapon.transform.Translate(0.875f,0.0f,0.0f);
            weapon.transform.parent = this.transform;
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }

        //Check if collision object is an ammo pack single
        if (collision.gameObject.tag == "AmmoSingleWeapon")
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
            this.health += collision.gameObject.GetComponent<SetHealthPack>().healthAmount;
            if (this.health > this.maxHealth){
                this.health = this.maxHealth;
            }
            this.gameController.UpdatePlayerHealth(this.health);
            Debug.Log(this.health);
            Destroy(collision.gameObject);
        }

        //Check if collision object is an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            this.Damage(10);
            Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();
            Vector3 direction = (transform.position - collision.gameObject.transform.position).normalized;

            this.activeKnockBacks.Add(new KnockBack(direction, 0.4f, 0.1f));
        }
    }

    public void Damage(int damageAmount) {
        this.health -= damageAmount;
        this.gameController.UpdatePlayerHealth(this.health);
        if (this.health <= 0){
            this.Kill();
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
        this.animator.SetBool("Dead", true);
        this.GetActiveWeapon().GetComponent<SpriteRenderer>().enabled = false;
        this.GetComponent<Rigidbody2D>().simulated = false;
        this.GetComponent<PlayerUserInput>().enabled = false;
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
        return this.GetHeldWeapons()[activeWeaponIndex];  
    }

    public Weapon GetSecondaryWeapon(){
        return this.GetHeldWeapons()[(activeWeaponIndex + 1) % this.GetHeldWeapons().Count];
    }

    public int GetHealth() {
        return this.health;
    }
}