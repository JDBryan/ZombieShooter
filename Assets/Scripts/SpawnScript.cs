using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    private float timeBetweenSpawn = 2f;
    private int enemyNumber = 5;
    private float countdown = 5f;
    private Vector3 spawnerSize;
    private BoxCollider2D myBox;
    public GameObject newEnemy;
    public GameObject controller;


    // Start is called before the first frame update
    void Start()
    {
        //myRenderer = GetComponent<MeshRenderer>();
        spawnerSize = GetComponent<BoxCollider2D>().size;
        countdown = timeBetweenSpawn;
        enemyNumber = controller.GetComponentInParent<WaveControllerScript>().waveAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (controller.GetComponentInParent<WaveControllerScript>().waveOn != true){
            enemyNumber = controller.GetComponentInParent<WaveControllerScript>().waveAmount;
        }
        else{
            Wave();
        }   
    }

    void Spawn()
    {
        if (countdown <= 0){
            GameObject enemy = Instantiate(newEnemy, FindSpawnPoint(), this.transform.rotation);
            enemyNumber -= 1;
            countdown = timeBetweenSpawn;
        }
        else{
            countdown -= Time.deltaTime;
        }
    }

    void Wave()
    {
        if (enemyNumber > 0){
            
            Spawn();
        }
    }

    Vector3 FindSpawnPoint()
    {
        float minX = this.transform.position.x-spawnerSize.x;
        float maxX = this.transform.position.x+spawnerSize.x;
        float minY = this.transform.position.y-spawnerSize.y;
        float maxY = this.transform.position.y+spawnerSize.y;
        Vector3 spawnPoint = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);

        return spawnPoint;
    }
}
