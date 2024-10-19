using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : MonoBehaviour
{
    // Parameters
    [SerializeField] private int maxHealth;
    [SerializeField] private float speed;
    [SerializeField] private float speedVariance;
    [SerializeField] private float rotateSpeed;
    [SerializeField] public int moneyForPlayer;
    [SerializeField] private float chanceToDropHealth;

    // Tracking
    [HideInInspector] private int currentHealth;
    [HideInInspector] private bool spawnEnded;
    [HideInInspector] private Vector3 target;

    // Object references
    [SerializeField] private GameObject bloodSplat;
    [SerializeField] private GameObject healthPack;
    public static event Action<Enemy> OnKilled;

    private void Start() {
        // Tracking
        this.spawnEnded = false;
        this.currentHealth = this.maxHealth;
        this.speed += UnityEngine.Random.Range(-this.speedVariance, this.speedVariance);
        this.target = Player.Instance.transform.position;
    }

    private void FixedUpdate() {
        if (Pathfinder.Instance.CheckIfCellInCameFrom(this.gameObject.transform.position)){
            this.target = Pathfinder.Instance.NextDestination(this.gameObject.transform.position);
        }
        RotateTowardsTarget();
        if (spawnEnded == true) {
            this.GetComponent<Rigidbody2D>().MovePosition(transform.position + transform.up * speed);
        }
    }

    private void RotateTowardsTarget() {
        Vector2 targetDirection = target - transform.position;
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle));
        transform.localRotation = Quaternion.Slerp(transform.localRotation, q, rotateSpeed * Time.deltaTime);
    }

    public void Damage(int damageAmount, Quaternion bulletRotation) {
        if (this.currentHealth > 0){
            this.currentHealth -= damageAmount;
            if (this.currentHealth <= 0) {
                Instantiate(this.bloodSplat, this.transform.position, bulletRotation);
                if (UnityEngine.Random.Range(0f,1f) <= chanceToDropHealth){
                    GameObject pack = Instantiate(healthPack, this.transform.position, Quaternion.identity);
                    pack.GetComponent<HealthPack>().AddKnockBack(bulletRotation * Vector3.up, 0.2f, 0.4f);
                }
                OnKilled.Invoke(this);
                Destroy(this.gameObject);
            }
        }
    }

    public void SpawnEnded() {
        spawnEnded = true;
    }
}
