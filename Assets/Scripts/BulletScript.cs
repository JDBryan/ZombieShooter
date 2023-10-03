using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        //Check for a match with the specific tag on any GameObject that collides with your GameObject
        if (collision.gameObject.tag == "Obstacle")
        {
            Destroy(this.gameObject);
        } else if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyMoveScript>().Damage(10);
            Destroy(this.gameObject);
        }
    }
}
