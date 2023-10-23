using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Spawner spawnController;
    private int waveNumber;
    private bool waveInProgress;
    private int baseSpawnAmount;
    private int incrementalSpawnAmount;
    private int waveKillCount;

    void Start()
    {
        this.waveNumber = 0;
        this.waveKillCount = 0;
        this.waveInProgress = false;
        this.baseSpawnAmount = 2;
        this.incrementalSpawnAmount = 1;
        this.spawnController.SetSpawnInterval(2);
    }

    void Update()
    {        
        if (!this.waveInProgress && Input.GetKeyDown("space")) {
            this.StartWave();
        }
    }

    private void StartWave() {
        this.waveInProgress = true;
        this.waveKillCount = 0;
        this.waveNumber += 1;
        this.spawnController.EnqueueSpawns(this.GetWaveSpawnTotal());
    }

    private void EndWave() {
        this.waveInProgress = false;
    }

    // Increments wave kill count by one. Checks if the kill count for the wave is equal
    // to the number of enemies that we intended to spawn. If so ends wave
    public void RegisterEnemyDeath()
    {
        this.waveKillCount += 1;
        if (this.waveKillCount == this.GetWaveSpawnTotal()) {
            this.EndWave();
        }
    }

    public int GetWaveNumber() {
        return this.waveNumber;
    }

    private int GetWaveSpawnTotal() {
       return (this.waveNumber - 1 * this.incrementalSpawnAmount) + this.baseSpawnAmount;
    }

    public void EndGame() {

    }
}
