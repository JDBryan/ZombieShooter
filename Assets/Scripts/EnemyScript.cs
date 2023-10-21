using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target;
    private int speed;
    private float rotateSpeed;
    private Rigidbody2D rigidBody;
    private int health;

    private void Start() {
        this.speed = 6;
        this.rotateSpeed = 4f;
        this.health = 100;
        this.rigidBody = GetComponent<Rigidbody2D>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (health <= 0) {
            Destroy(this.gameObject);
        }
        RotateTowardsTarget();
        rigidBody.MovePosition(transform.position + transform.up * speed * Time.deltaTime);
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target.position - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed * Time.deltaTime);
    }

    public void Damage(int damageAmount) {
        health -= damageAmount;
    }
}
