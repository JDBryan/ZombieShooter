using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform target;
    private GameController gameController;
    private float speed;
    private float rotateSpeed;
    private Rigidbody2D rigidBody;
    private int health;
    private bool spawnEnded;
    public GameObject BloodSplat;

    private void Start() {
        this.speed = 0.06f;
        this.spawnEnded = false;
        this.rotateSpeed = 4f;
        this.health = 100;
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
        this.gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    private void FixedUpdate() {
        RotateTowardsTarget();
        if (spawnEnded == true) {
            rigidBody.MovePosition(transform.position + transform.up * speed);
        }
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed * Time.deltaTime);
    }

    public void Damage(int damageAmount, Quaternion bulletRotation) {
        health -= damageAmount;
        if (health <= 0) {
            Instantiate(BloodSplat, this.transform.position, bulletRotation);
            gameController.RegisterEnemyDeath();
            Destroy(this.gameObject);
        }
    }

    public void SpawnEnded() {
        spawnEnded = true;
    }
}
