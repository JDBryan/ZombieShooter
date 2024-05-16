using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static System.Random;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject smallEnemy;

    [SerializeField] private GameObject bigEnemy;

    [SerializeField] private List<Transform> spawnAreas;
    private int spawnInterval;
    private int smallEnemySpawnQueue;
    private int bigEnemySpawnQueue;

    private float lastSpawnTime;
    private GameObject AllEnemies;

    void Start()
    {
        this.AllEnemies = new GameObject();
        foreach(Transform spawnArea in transform) {
            spawnAreas.Add(spawnArea);
        }
        lastSpawnTime = Time.time;
    }

    void Update() 
    {
        // Per spawn interval, if the spawnQueue is not empty, spawn a single enemy
        // and decrement the spawn queue
        if (smallEnemySpawnQueue + bigEnemySpawnQueue > 0 && Time.time - lastSpawnTime >= spawnInterval) {
            this.SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    public void KillAllEnemies() {
        Destroy(this.AllEnemies);
        this.AllEnemies = new GameObject();
    }

    // Spawns a single enemy within the bounds of one of the spawn areas
    private void SpawnEnemy()
    {
        // Get a random spawn area from the list of spawn areas 
        System.Random random = new System.Random();
        Transform spawnArea = spawnAreas[random.Next(spawnAreas.Count)].transform;

        // Find a random point within this spawn area
        Vector3 extents = spawnArea.GetComponent<SpriteRenderer>().bounds.extents; 

        float minX = spawnArea.position.x - extents.x;
        float maxX = spawnArea.position.x + extents.x;
        float minY = spawnArea.position.y - extents.y;
        float maxY = spawnArea.position.y + extents.y;
        Vector3 spawnPoint = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);

        int total = this.smallEnemySpawnQueue + this.bigEnemySpawnQueue;
        int choice = Random.Range(1, total + 1);
        if (choice <= this.smallEnemySpawnQueue) {
                Instantiate(smallEnemy, spawnPoint, this.transform.rotation, AllEnemies.transform);
                this.smallEnemySpawnQueue -= 1;
        } else {
                Instantiate(bigEnemy, spawnPoint, this.transform.rotation, AllEnemies.transform);
                this.bigEnemySpawnQueue -= 1;
        }
    }

    public void SetSpawnInterval(int interval) 
    {
        this.spawnInterval = interval;
    }

    public void EnqueueSmallEnemySpawns(int spawnAmount) 
    {
        this.smallEnemySpawnQueue += spawnAmount;
    }
    
    public void EnqueueBigEnemySpawns(int spawnAmount) 
    {
        this.bigEnemySpawnQueue += spawnAmount;
    }
}
