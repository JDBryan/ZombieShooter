using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum GameState 
{
  Active,
  Paused,
  Over,
  StartMenu
}

public class GameController : MonoBehaviour
{
    [SerializeField] private Spawner spawnController;
    [SerializeField] private UserInterface userInterface;
    [SerializeField] private Player player;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private PlayerCamera camera;
    private GameState gameState;
    private int waveNumber;
    private bool waveInProgress;
    private int baseSpawnAmount;
    private int incrementalSpawnAmount;
    private int waveKillCount;
    private float lastWaveEndedTime;
    private int waveIntervalTime;

    void Start()
    {
        this.gameState = GameState.StartMenu;
        this.baseSpawnAmount = 2;
        this.incrementalSpawnAmount = 1;
        this.waveIntervalTime = 5;
        this.spawnController.SetSpawnInterval(2);
    }

    void Update()
    {   
        if (this.gameState == GameState.StartMenu && Input.GetKeyDown("space")) {
            this.StartGame();
        }

        if (this.gameState == GameState.Over && Input.GetKeyDown("space")) {
            this.ResetGame();
        }

        if (this.gameState == GameState.Active && !this.waveInProgress) {
            if (Time.time - this.lastWaveEndedTime >= this.waveIntervalTime) {
                this.StartWave();
            }
        }
    }

    private void StartGame() {
        this.userInterface.Reset();
        this.gameState = GameState.Active;
        this.userInterface.SetStartMenuActive(false);
        this.player.EnableUserInput();
        this.lastWaveEndedTime = Time.time;
        this.userInterface.EnableHud();
    }

    public void EndGame() {
        this.gameState = GameState.Over;
        this.userInterface.DisableHud();
        this.userInterface.SetGameOverScreenActive(true);
    }

    public void ResetGame() {
        this.waveNumber = 0;
        this.waveKillCount = 0;
        this.waveInProgress = false;
        this.spawnController.KillAllEnemies();
        this.gameState = GameState.StartMenu;
        this.userInterface.SetGameOverScreenActive(false);
        this.userInterface.SetStartMenuActive(true);
        Destroy(this.player.gameObject);
        this.player = Instantiate(this.playerPrefab);
        this.camera.GetNewTransform();        
    }

    private void StartWave() {
        Debug.Log("Starting wave");
        this.waveInProgress = true;
        this.waveKillCount = 0;
        this.waveNumber += 1;
        this.spawnController.EnqueueSpawns(this.GetWaveSpawnTotal());
        this.userInterface.SetWaveNumber(this.waveNumber);
    }

    private void EndWave() {
        this.lastWaveEndedTime = Time.time;
        this.waveInProgress = false;
    }

    // Increments wave kill count by one. Checks if the kill count for the wave is equal
    // to the number of enemies that we intended to spawn. If so ends wave.
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

    public void UpdatePlayerHealth(int health) {
        this.userInterface.UpdateHealthBar();
        if (health <= 0) {
            this.EndGame();
        }
    }
}
