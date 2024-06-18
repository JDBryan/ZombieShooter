using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public enum GameState 
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
    [SerializeField] private PlayerCamera playerCamera;
    [SerializeField] private AudioClip deathNoise;
    public GameObject bloodParent;
    [SerializeField] private GameObject doorPrefab;
    [HideInInspector] public GameState gameState;
    private int waveNumber;
    private bool waveInProgress;
    private int smallEnemyBaseSpawnAmount;
    private int bigEnemyBaseSpawnAmount;

    private float smallEnemyIncrementalSpawnAmount;
    private float bigEnemyIncrementalSpawnAmount;
    private int waveKillCount;
    private float lastWaveEndedTime;
    private int waveIntervalTime;

    void Start()
    {
        this.gameState = GameState.StartMenu;
        this.smallEnemyBaseSpawnAmount = 2;
        this.bigEnemyBaseSpawnAmount = 1;
        this.smallEnemyIncrementalSpawnAmount = 1;
        this.bigEnemyIncrementalSpawnAmount = 0.5f;
        this.waveIntervalTime = 5;
        this.spawnController.SetSpawnInterval(2);
    }

    void Update()
    {   
        if (this.gameState == GameState.Active && Input.GetKeyDown(KeyCode.Escape)) {
            this.PauseGame();
        }

        else if (this.gameState == GameState.Paused && Input.GetKeyDown(KeyCode.Escape)) {
            this.ResumeGame();
        }

        if (this.gameState == GameState.Active && !this.waveInProgress) {
            if (Time.time - this.lastWaveEndedTime >= this.waveIntervalTime) {
                this.StartWave();
            }
        }
    }

    public void StartGame() {
        this.userInterface.EnableHud();
        this.userInterface.Reset();
        this.gameState = GameState.Active;
        this.userInterface.SetStartMenuActive(false);
        this.player.EnableUserInput();
        this.lastWaveEndedTime = Time.time;
    }

    public void EndGame() {
        this.gameState = GameState.Over;
        this.userInterface.DisableHud();
        this.userInterface.SetGameOverScreenActive(true);
    }

    public void ResetGame() {
        Time.timeScale = 1;
        this.userInterface.SetPauseMenuActive(false);
        this.waveNumber = 0;
        this.waveKillCount = 0;
        this.waveInProgress = false;
        this.spawnController.KillAllEnemies();
        this.spawnController.ClearSpawnQueues();
        this.gameState = GameState.StartMenu;
        this.userInterface.SetGameOverScreenActive(false);
        this.userInterface.SetStartMenuActive(true);
        this.DestroyBloodSplats();
        this.DestroyHealthPacks();
        Destroy(this.player.gameObject);
        this.player = Instantiate(this.playerPrefab);
        this.playerCamera.GetNewTransform(); 
        foreach (WeaponStation station in GameObject.FindObjectsByType<WeaponStation>(FindObjectsSortMode.None)){
            station.ResetStation();
        } 
        this.ResetAreaDoors();
        this.gameObject.GetComponent<Pathfinder>().Reset();
    }

    public void PauseGame() {
        Time.timeScale = 0;
        this.player.DisableUserInput();
        this.gameState = GameState.Paused;
        this.userInterface.SetPauseMenuActive(true);
        this.userInterface.DisableHud();
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        this.player.EnableUserInput();
        this.gameState = GameState.Active;
        this.userInterface.SetPauseMenuActive(false);
        this.userInterface.EnableHud();
    }

    private void StartWave() {
        Debug.Log("Starting wave");
        this.waveInProgress = true;
        this.waveKillCount = 0;
        this.waveNumber += 1;
        this.spawnController.EnqueueSmallEnemySpawns(this.GetSmallEnemyWaveSpawnTotal());
        this.spawnController.EnqueueBigEnemySpawns(this.GetBigEnemyWaveSpawnTotal());
        this.userInterface.SetWaveNumber(this.waveNumber);
    }

    private void EndWave() {
        this.lastWaveEndedTime = Time.time;
        this.waveInProgress = false;
    }

    // Increments wave kill count by one. Checks if the kill count for the wave is equal
    // to the number of enemies that we intended to spawn. If so ends wave.
    public void RegisterEnemyDeath(Enemy enemy)
    {
        GetComponent<AudioSource>().PlayOneShot(deathNoise);
        this.player.ChangePlayerMoney(enemy.moneyForPlayer);
        this.waveKillCount += 1;
        if (this.waveKillCount == this.GetSmallEnemyWaveSpawnTotal() + this.GetBigEnemyWaveSpawnTotal()) {
            this.EndWave();
        }
    }

    public int GetWaveNumber() {
        return this.waveNumber;
    }

    private int GetSmallEnemyWaveSpawnTotal() {
       return (int)((this.waveNumber - 1) * this.smallEnemyIncrementalSpawnAmount) + this.smallEnemyBaseSpawnAmount;
    }

    private int GetBigEnemyWaveSpawnTotal() {
       return (int)(((this.waveNumber - 1) * this.bigEnemyIncrementalSpawnAmount)) + this.bigEnemyBaseSpawnAmount;
    }

    public void UpdatePlayerHealth(int health) {
        this.userInterface.UpdateHealthBar();
        if (health <= 0) {
            this.EndGame();
        }
    }

    private void DestroyBloodSplats(){
        foreach (Transform splat in this.bloodParent.transform){
            Destroy(splat.gameObject);
        }
    }

    private void DestroyHealthPacks(){
        foreach (HealthPack pack in GameObject.FindObjectsByType<HealthPack>(FindObjectsSortMode.None)){
            Destroy(pack.gameObject);
        }
    }

    private void ResetAreaDoors(){
        Transform doorParent = GameObject.Find("AreaDoors").transform;
        List<Transform> locations = new List<Transform>();
        foreach (AreaDoor door in GameObject.FindObjectsByType<AreaDoor>(FindObjectsSortMode.None)){
            locations.Add(door.transform);
            Destroy(door.gameObject);
        }
        foreach (Transform newTransform in locations){
            Instantiate(doorPrefab, newTransform.position, newTransform.rotation, doorParent);
        }
    }
}
