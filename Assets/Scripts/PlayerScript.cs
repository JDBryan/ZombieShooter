using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private GameController gameController;
    [SerializeField] private Animator animator;
    private float health;
    private float speed;
    private int activeWeaponIndex;

    void Start()
    {
        activeWeaponIndex = 0;
        health = 100;
        speed = 0.14f;
    }
    
    void Update()
    {	
        // Getting inputs for movement and mouseposition
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Updating movement and mouse position
        // UpdateMovement(horizontal, vertical);
        UpdateRotation(mousePosition);

        Vector2 velocity = new Vector2(horizontal, vertical); 

        animator.SetFloat("Speed", velocity.magnitude);

        //Checking for use inputs
        if (Input.GetMouseButtonDown(0)) {
            this.GetActiveWeapon().Fire(this.transform);
        }

        if (Input.GetKeyDown("e")) {
            activeWeaponIndex = (activeWeaponIndex + 1) % this.GetHeldWeapons().Count;
        }
    }

    void FixedUpdate() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        UpdateMovement(horizontal, vertical);
    }

    void UpdateMovement(float horizontal, float vertical) 
    {
        Vector3 movement = new Vector3(horizontal, vertical, 0);
        float magnitude = movement.magnitude;
        Rigidbody2D rigidBody = this.GetComponent<Rigidbody2D>();
        if (magnitude != 0) {
            float scalar = speed / magnitude;
            Vector3 scaledMovement = movement * scalar;
            rigidBody.MovePosition(transform.position + scaledMovement);
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
            weapon.transform.parent = this.transform;
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }

        //Check if collision object is an enemy
        if (collision.gameObject.tag == "Enemy")
        {
            this.health -= 10;
        }
    }

    public void Damage(int damageAmount) {
        this.health -= 10;
        if (this.health <= 0) {
            this.gameController.EndGame();
        }
    }

    // Helper function to calculate the angle from point a to point b
    float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

    // Returns a list of the currently held weapons 
    public List<Weapon> GetHeldWeapons() {
        List<Weapon> heldWeapons = new List<Weapon>();
        foreach (Transform weapon in this.transform) {
            if(weapon.tag != "MainCamera") {
                heldWeapons.Add(weapon.GetComponent<Weapon>());
            }
        }
        return heldWeapons;
    }

    public Weapon GetActiveWeapon()
    {
      return this.GetHeldWeapons()[activeWeaponIndex];  
    }
}

