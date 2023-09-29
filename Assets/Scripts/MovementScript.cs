using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private bool hasGun = false;
    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0,0,270);
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0);
		
		//Get the Screen position of the mouse and player
        Vector2 playerPosition = Camera.main.WorldToViewportPoint(transform.position);
		Vector2 mousePosition = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
		
		//Get the angle between the player and the camera
		float angle = AngleBetweenTwoPoints(playerPosition, mousePosition);

		//Perform translation and rotation
		transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, angle+90));
        transform.position += movement * Time.deltaTime;

        if (Input.GetMouseButtonDown(0)) {
            if (hasGun) {
                Debug.Log("Bang");
            } else {
                Debug.Log("Swish");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Pickup")
        {
            Debug.Log("Do something here");
            Destroy(collision.gameObject);
            hasGun = true;
        }
    }

    void OnMouseDown()
    {
        if (hasGun) {
            Debug.Log("Bang");
        } else {
            Debug.Log("Swish");
        }
    }

    float AngleBetweenTwoPoints(Vector2 a, Vector2 b) {
		return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
	}
}

