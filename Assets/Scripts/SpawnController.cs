using System.Collections.Generic;
using UnityEngine;
public class SpawnController : MonoBehaviour
{
    [SerializeField] private GameObject smallEnemy;
    [SerializeField] private GameObject bigEnemy;
    [SerializeField] private Transform spawnAreaParent;
    [SerializeField] private int spawnInterval;
    [HideInInspector] private List<Transform> activeSpawnAreas;
    [HideInInspector] private int smallEnemySpawnQueue;
    [HideInInspector] private int bigEnemySpawnQueue;
    [HideInInspector] private float lastSpawnTime;
    [HideInInspector] private GameObject AllEnemies;
    [SerializeField] private int smallEnemyBaseSpawnAmount;
    [SerializeField] private int bigEnemyBaseSpawnAmount;
    [SerializeField] private float smallEnemyIncrementalSpawnAmount;
    [SerializeField] private float bigEnemyIncrementalSpawnAmount;

    // Singleton instance
    public static SpawnController Instance { get; private set; }

    void Start()
    {
        Instance = this;
        this.AllEnemies = new GameObject();
        activeSpawnAreas = new List<Transform>();
        ActivateRoomSpawning("Cockpit");
        lastSpawnTime = Time.time;
        this.smallEnemyBaseSpawnAmount = 2;
        this.bigEnemyBaseSpawnAmount = 1;
        this.smallEnemyIncrementalSpawnAmount = 1;
        this.bigEnemyIncrementalSpawnAmount = 0.5f;
        this.spawnInterval = 2;

        // Register event handlers
        GameController.OnWaveStarted += QueueSpawnsForWave;
        GameController.OnGameReset += Reset;
    }

    void Update() 
    {
        if (smallEnemySpawnQueue + bigEnemySpawnQueue > 0 && Time.time - lastSpawnTime >= spawnInterval) {
            this.SpawnEnemy();
            lastSpawnTime = Time.time;
        }
    }

    public void ActivateRoomSpawning(string roomName){
        foreach(Transform spawnArea in spawnAreaParent.Find(roomName)) {
            activeSpawnAreas.Add(spawnArea.transform);
        }
    }

    public void Reset() 
    {
        // Kill all enemies
        Destroy(this.AllEnemies);
        this.AllEnemies = new GameObject();

        // Clear spawn queues
        this.smallEnemySpawnQueue = 0;
        this.bigEnemySpawnQueue = 0;

        // Reset spawn areas
        activeSpawnAreas.Clear();
        ActivateRoomSpawning("Cockpit");
    }

    public void QueueSpawnsForWave(int waveNumber) {
        this.smallEnemySpawnQueue += this.GetSmallEnemyWaveSpawnTotal(waveNumber);
        this.bigEnemySpawnQueue += this.GetBigEnemyWaveSpawnTotal(waveNumber);
    }

    // Spawns a single enemy within the bounds of one of the spawn areas
    private void SpawnEnemy()
    {
        // Get a random spawn area from the list of spawn areas 
        System.Random random = new System.Random();
        Transform spawnArea = activeSpawnAreas[random.Next(activeSpawnAreas.Count)].transform;

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
    public int GetWaveSpawnTotal(int waveNumber) {
        return GetSmallEnemyWaveSpawnTotal(waveNumber) + GetBigEnemyWaveSpawnTotal(waveNumber);
    }

    private int GetSmallEnemyWaveSpawnTotal(int waveNumber) {
       return (int)((waveNumber - 1) * this.smallEnemyIncrementalSpawnAmount) + this.smallEnemyBaseSpawnAmount;
    }

    private int GetBigEnemyWaveSpawnTotal(int waveNumber) {
       return (int)((waveNumber - 1) * this.bigEnemyIncrementalSpawnAmount) + this.bigEnemyBaseSpawnAmount;
    }
}
