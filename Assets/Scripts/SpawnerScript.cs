using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private List<Transform> spawnAreas;
    private int spawnInterval;
    private int spawnQueue;
    private float lastSpawnTime;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform spawnArea in transform) {
            spawnAreas.Add(spawnArea);
        }
        lastSpawnTime = Time.time;
    }

    void Update() {
        if (spawnQueue > 0 && Time.time - lastSpawnTime >= spawnInterval) {
            this.SpawnEnemy();
            spawnQueue -= 1;
            lastSpawnTime = Time.time;
        }
    }

    public void SpawnEnemy()
    {
        // Get a random spawn area from our list of spawn areas 
        System.Random random = new System.Random();
        Transform spawnArea = spawnAreas[random.Next(spawnAreas.Count)].transform;

        Vector3 extents = spawnArea.GetComponent<SpriteRenderer>().bounds.extents;        

        // Find a random point within this spawn area
        float minX = spawnArea.position.x - extents.x;
        float maxX = spawnArea.position.x + extents.x;
        float minY = spawnArea.position.y - extents.y;
        float maxY = spawnArea.position.y + extents.y;
        Vector3 spawnPoint = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);

        Instantiate(enemy, spawnPoint, this.transform.rotation);
    }

    public void SetSpawnInterval(int interval) {
        this.spawnInterval = interval;
    }

    public void EnqueueSpawns(int spawnAmount) {
        this.spawnQueue += spawnAmount;
    }

    public bool IsSpawnQueueEmpty() {
        return this.spawnQueue == 0;
    }
}
