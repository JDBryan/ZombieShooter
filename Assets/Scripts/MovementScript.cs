using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using System.Rigidbody2D

public class Movement : MonoBehaviour
{
    private bool hasGun = false;
    private float health = 100f;
    private float speed = 2f;
    public GameObject bulletPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0,0,270);
    }

    // Update is called once per frame
    void Update()
    {	
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        UpdateMovement(horizontal, vertical);
        UpdateRotation();

        if (Input.GetMouseButtonDown(0)) {
            if (hasGun) {
                this.FireGun();
            } else {
                Debug.Log("Swish");
            }
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
            Destroy(collision.gameObject);
            hasGun = true;
        }
    }

    void FireGun() {
        GameObject bullet = Instantiate(bulletPrefab, transform.position, transform.rotation);
        Physics2D.IgnoreCollision(bullet.GetComponent<BoxCollider2D>(), GetComponent<CircleCollider2D>());
        Rigidbody2D bulletBody = bullet.GetComponent<Rigidbody2D>();
        bulletBody.AddForce(bulletBody.transform.up * 4000);
    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
}

