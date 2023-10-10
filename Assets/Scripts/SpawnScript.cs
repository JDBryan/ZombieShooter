using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnScript : MonoBehaviour
{
    public float timeBetweenWave = 5f;
    private int enemyNumber = 1;
    private float countdown = 5f;
    private Vector3 spawnerSize;
    private MeshRenderer myRenderer;
    public GameObject newEnemy;


    // Start is called before the first frame update
    void Start()
    {
        myRenderer = GetComponent<MeshRenderer>();
        spawnerSize = GetComponent<Renderer>().bounds.extents;

    }

    // Update is called once per frame
    void Update()
    {
        if (countdown <= 0){
            GameObject enemy = Instantiate(newEnemy, FindSpawnPoint(), this.transform.rotation);
            countdown = timeBetweenWave;
        }
        else{
            countdown -= Time.deltaTime;
        }
    }

    Vector3 FindSpawnPoint()
    {
        float minX = this.transform.position.x-spawnerSize.x;
        float maxX = this.transform.position.x+spawnerSize.x;
        float minY = this.transform.position.y-spawnerSize.y;
        float maxY = this.transform.position.y-spawnerSize.y;
        Vector3 spawnPoint = new Vector3(Random.Range(minX, maxX), Random.Range(minY, maxY), 0f);

        return spawnPoint;
    }
}
