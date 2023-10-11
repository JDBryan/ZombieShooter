using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator animator;
    private float health = 100f;
    private float speed = 2f;
    private int activeWeaponIndex;

    void Start()
    {
        activeWeaponIndex = 0;
    }
    
    void Update()
    {	
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        UpdateMovement(horizontal, vertical);
        UpdateRotation();

        Vector2 velocity = new Vector2(horizontal, vertical); 

        animator.SetFloat("Speed", velocity.magnitude);

        if (Input.GetMouseButtonDown(0)) {
            this.GetActiveWeapon().Fire(this.transform);
        }

        if (Input.GetKeyDown("e")) {
            activeWeaponIndex = (activeWeaponIndex + 1) % this.GetHeldWeapons().Count;
        }
    }

    void UpdateMovement(float horizontal, float vertical) 
    {
        Vector3 movement = new Vector3(horizontal, vertical, 0);
        float magnitude = movement.magnitude;
        if (magnitude != 0) {
            float scalar = speed / magnitude;
            Vector3 scaledMovement = movement * scalar * Time.deltaTime;
            transform.position += scaledMovement;
        }
    }

    void UpdateRotation()
    {
        //Get the Screen position of the mouse and player
        Vector2 playerPosition = (Vector2)transform.position;
		Vector2 mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		
		//Get the angle between the player and the camera
		float angle = AngleBetweenTwoPoints(playerPosition, mousePosition);

		//Perform translation and rotation
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Pickup")
        {
            Debug.Log("Picking up weapon!");
            GameObject weapon = collision.gameObject;
            weapon.transform.parent = this.transform;
            weapon.GetComponent<BoxCollider2D>().enabled = false;
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}

    public List<Weapon> GetHeldWeapons() {
        List<Weapon> heldWeapons = new List<Weapon>();
        foreach (Transform weapon in this.transform) {
            if(weapon.tag != "MainCamera") {
                heldWeapons.Add(weapon.GetComponent<Weapon>());
            }
        }
        Debug.Log(heldWeapons.Count);
        return heldWeapons;
    }

    public Weapon GetActiveWeapon()
    {
      return this.GetHeldWeapons()[activeWeaponIndex];  
    }
}

