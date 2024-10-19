using System;
using System.Collections.Generic;
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
    [SerializeField] private UserInterface userInterface;
    [SerializeField] private Player playerPrefab;
    [SerializeField] private AudioClip deathNoise;
    [SerializeField] private GameObject doorsPrefab;
    [SerializeField] private GameObject roomDarknessPrefab;
    private GameObject roomDarkness;
    [HideInInspector] public GameState gameState;
    private int waveNumber;
    private bool waveInProgress;
    private int waveKillCount;
    private float lastWaveEndedTime;
    private int waveIntervalTime;
    public Dictionary<string, bool> roomsDict;

    // Singleton instance
    public static GameController Instance { get; private set; }

    // Events
    // Fired when a new wave starts, int representing new wave number
    public static event Action<int> OnWaveStarted;
    // Fired when the game is reset - returning to start menu
    public static event Action OnGameReset;
    // Fired when the game is started - play begins
    public static event Action OnGameStart;
    // Fired when game is paused
    public static event Action OnGamePaused;
    // Fired when game is resumed from pause
    public static event Action OnGameResumed;

    void Start()
    {
        Instance = this;
        this.gameState = GameState.StartMenu;
        this.waveIntervalTime = 5;
        this.roomsDict = new Dictionary<string, bool>(){
            {"Cockpit", true},
            {"WeaponsBay", false},
            {"CentralRoom", false},
            {"MedBay", false},
            {"EngineRoom", false}
        };
        this.roomDarkness = Instantiate(roomDarknessPrefab);
        this.roomDarkness.name = roomDarknessPrefab.name;
        Enemy.OnKilled += RegisterEnemyDeath;
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
        this.gameState = GameState.Active;
        OnGameStart.Invoke();
        this.lastWaveEndedTime = Time.time;
    }

    //Triggered in Player
    public void EndGame() { 
        this.gameState = GameState.Over;
        PlayerCamera.Instance.DeathCameraAnimation(true);
    }

    //Triggered in Player Camera
    public void SetDeathMenuActive()
    { 
        this.userInterface.SetGameOverScreenActive(true);
    }

    public void ResetGame() 
    {
        Time.timeScale = 1;

        this.waveNumber = 0;
        this.waveKillCount = 0;
        this.waveInProgress = false;
        this.gameState = GameState.StartMenu;
        this.ResetAreaDoors();
        this.gameObject.GetComponent<Pathfinder>().Reset();
        Destroy(this.roomDarkness);
        this.roomDarkness = Instantiate(roomDarknessPrefab);
        this.roomDarkness.name = roomDarknessPrefab.name;
        
        // Create new player
        Destroy(Player.Instance.gameObject);
        Instantiate(this.playerPrefab);

        OnGameReset.Invoke();
    }

    public void PauseGame() 
    {
        Time.timeScale = 0;
        this.gameState = GameState.Paused;
        OnGamePaused.Invoke();
    }

    public void ResumeGame() 
    {
        Time.timeScale = 1;
        this.gameState = GameState.Active;
        OnGameResumed.Invoke();
    }

    private void StartWave() 
    {
        this.waveInProgress = true;
        this.waveKillCount = 0;
        this.waveNumber += 1;
        OnWaveStarted.Invoke(this.waveNumber);
    }

    private void EndWave() 
    {
        this.lastWaveEndedTime = Time.time;
        this.waveInProgress = false;
    }

    // Increments wave kill count by one. Checks if the kill count for the wave is equal
    // to the number of enemies that we intended to spawn. If so ends wave.
    public void RegisterEnemyDeath(Enemy enemy)
    {
        GetComponent<AudioSource>().PlayOneShot(deathNoise);
        Player.Instance.ChangePlayerMoney(enemy.moneyForPlayer);
        this.waveKillCount += 1;
        if (this.waveKillCount == SpawnController.Instance.GetWaveSpawnTotal(this.waveNumber)) {
            this.EndWave();
        }
    }

    public void UpdateRooms(List<string> roomNames)
    {
        foreach (string roomName in roomNames){
            if (!roomsDict[roomName]){
                roomsDict[roomName] = true;
                this.roomDarkness.transform.Find(roomName).GetComponent<Animator>().SetBool("Open", true);
                SpawnController.Instance.ActivateRoomSpawning(roomName);
            }
        }
    }

    private void ResetAreaDoors()
    {
        Destroy(GameObject.Find("AreaDoors").gameObject);
        GameObject newDoors = Instantiate(doorsPrefab);
        newDoors.name = doorsPrefab.name;
    }
}
