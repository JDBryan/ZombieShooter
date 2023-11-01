using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Animator animator;
    private int health;
    private float speed;
    private int activeWeaponIndex;
    private Vector2 velocity;
    private List<KnockBack> activeKnockBacks;

    void Start()
    {
        activeWeaponIndex = 0;
        health = 100;
        speed = 0.14f;
        velocity = new Vector2(0, 0);
        activeKnockBacks = new List<KnockBack>();
    }
    
    void Update()
    {	
        // Getting user inputs
        this.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        

        if (Input.GetKeyDown("e")) {
            this.GetActiveWeapon().gameObject.GetComponent<SpriteRenderer>().enabled = false;
            activeWeaponIndex = (activeWeaponIndex + 1) % this.GetHeldWeapons().Count;
            this.GetActiveWeapon().gameObject.GetComponent<SpriteRenderer>().enabled = true;
        }
        
        animator.SetFloat("Speed", this.velocity.magnitude);
    }

    void LateUpdate() {
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        UpdateRotation(mousePosition);

        this.GetActiveWeapon().ChangeGunSpriteToIdle();

        if (Input.GetMouseButtonDown(0)) {
            this.GetActiveWeapon().Fire(this.transform);
            if (this.GetActiveWeapon().hasInfiniteAmmo || this.GetActiveWeapon().ammoCount > 0){
                this.GetActiveWeapon().ChangeGunSpriteToFire();
                }
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
    void UpdateRotation(Vector2 mousePosition)
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

    public int GetHealth() {
        return this.health;
    }

    
}

