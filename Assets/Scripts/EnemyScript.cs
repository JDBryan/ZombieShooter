using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

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
    [HideInInspector] private GameController gameController;
    [HideInInspector] private Pathfinder pathfinder;
    private Player player;

    private void Start() {
        // Tracking
        this.spawnEnded = false;
        this.currentHealth = this.maxHealth;
        this.speed += Random.Range(-this.speedVariance, this.speedVariance);
        this.player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        this.target = GameObject.FindGameObjectWithTag("Player").transform.position;

        // Object references
        this.gameController = GameObject.Find("GameController").GetComponent<GameController>();
        this.pathfinder = GameObject.Find("GameController").GetComponent<Pathfinder>();
    }

    private void FixedUpdate() {
        if (this.pathfinder.CheckIfCellInCameFrom(this.gameObject.transform.position)){
            this.target = this.pathfinder.NextDestination(this.gameObject.transform.position);
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
                if (Random.Range(0f,1f) <= chanceToDropHealth){
                    GameObject pack = Instantiate(healthPack, this.transform.position, Quaternion.identity);
                    pack.GetComponent<HealthPack>().AddKnockBack(bulletRotation * Vector3.up, 0.2f, 0.4f);
                }
                gameController.RegisterEnemyDeath(this);
                Destroy(this.gameObject);
            }
        }
    }

    public void SpawnEnded() {
        spawnEnded = true;
    }
}
