using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public Transform target;
    private float speed = 50f;
    private float rotateSpeed = 1.5f;
    private Rigidbody2D rb;
    private float health = 100f;
    private bool spawnEnded = false;

    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {

        if (!target) {
            GetTarget();
        } else {
            RotateTowardsTarget();
        }
        
        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (spawnEnded == true){
            rb.velocity = transform.up * speed * Time.deltaTime;
        }
        
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     Debug.Log("COLLISION");
    //     //Check for a match with the specific tag on any GameObject that collides with your GameObject
    //     if (collision.gameObject.tag == "Projectile")
    //     {
    //         Debug.Log("COLLISION WITH BULLET");
    //         health -= 50;
    //         Destroy(collision.gameObject);
    //     }
    // }

    public void HasSpawnEnded (){
        spawnEnded = true;
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed * Time.deltaTime);
    }

    private void GetTarget () {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    public void Damage(float damageAmount) {
        health -= damageAmount;
    }
}
