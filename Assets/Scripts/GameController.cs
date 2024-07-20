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
    [SerializeField] private GameObject doorsPrefab;
    [SerializeField] private GameObject roomDarknessPrefab;
    private GameObject roomDarkness;
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
    public Dictionary<string, bool> roomsDict;

    void Start()
    {
        this.gameState = GameState.StartMenu;
        this.smallEnemyBaseSpawnAmount = 2;
        this.bigEnemyBaseSpawnAmount = 1;
        this.smallEnemyIncrementalSpawnAmount = 1;
        this.bigEnemyIncrementalSpawnAmount = 0.5f;
        this.waveIntervalTime = 5;
        this.spawnController.SetSpawnInterval(2);
        this.roomsDict = new Dictionary<string, bool>(){
            {"Cockpit", true},
            {"WeaponsBay", false},
            {"CentralRoom", false},
            {"MedBay", false},
            {"EngineRoom", false}
        };
        this.roomDarkness = Instantiate(roomDarknessPrefab);
        this.roomDarkness.name = roomDarknessPrefab.name;
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

    public void EndGame() { //Gets triggered in Player
        this.gameState = GameState.Over;
        this.playerCamera.DeathCameraAnimation(true);
    }

    public void SetDeathMenuActive(){ //Triggered in Player Camera
        this.userInterface.SetGameOverScreenActive(true);
    }

    public void DeathScreen() {
        this.userInterface.DisableHud();
        this.player.GetComponent<SpriteRenderer>().sortingOrder = 10;
        this.player.ActivateDeathCircle();
        //Starts Camera move
        this.playerCamera.target = this.player.transform.position + Vector3.Normalize(this.player.transform.rotation * new Vector3(0f,1f,0f));
        this.playerCamera.isMoving = true;
    }

    public void ResetGame() {
        Time.timeScale = 1;
        this.playerCamera.DeathCameraAnimation(false);
        this.playerCamera.isMoving = false;
        this.userInterface.SetPauseMenuActive(false);
        this.waveNumber = 0;
        this.waveKillCount = 0;
        this.waveInProgress = false;
        this.spawnController.KillAllEnemies();
        this.spawnController.ClearSpawnQueues();
        this.spawnController.ResetRoomSpawnAreas();
        this.gameState = GameState.StartMenu;
        this.userInterface.SetGameOverScreenActive(false);
        this.userInterface.SetStartMenuActive(true);
        this.DestroyBloodSplats();
        this.DestroyHealthPacks();

        // Create new player
        Destroy(this.player.gameObject);
        this.player = Instantiate(this.playerPrefab);
        this.playerCamera.GetNewTransform(); 
        foreach (WeaponStation station in GameObject.FindObjectsByType<WeaponStation>(FindObjectsSortMode.None)){
            station.ResetStation();
        } 
        this.ResetAreaDoors();
        this.gameObject.GetComponent<Pathfinder>().Reset();
        Destroy(this.roomDarkness);
        this.roomDarkness = Instantiate(roomDarknessPrefab);
        this.roomDarkness.name = roomDarknessPrefab.name;
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
            this.DeathScreen();
        }
    }

    public void UpdateRooms(List<string> roomNames){
        foreach (string roomName in roomNames){
            if (!roomsDict[roomName]){
                roomsDict[roomName] = true;
                spawnController.ActivateRoomSpawning(roomName);
                this.roomDarkness.transform.Find(roomName).GetComponent<Animator>().SetBool("Open", true);
            }
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
        Transform doors = GameObject.Find("AreaDoors").transform;
        Destroy(doors.gameObject);
        GameObject newDoors = Instantiate(doorsPrefab);
        newDoors.name = doorsPrefab.name;
    }
}
