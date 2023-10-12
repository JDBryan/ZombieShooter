using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveControllerScript : MonoBehaviour
{
    public bool waveOn = true;
    public int waveAmount = 2;
    public float timeBetweenWave = 3;
    private bool canAddToWaveNum = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {        

        if (GetEnemyCount() > 0){
            canAddToWaveNum = true;
        }

        if (GetEnemyCount() == 0 && waveOn == true){
            waveOn = false;
            if (canAddToWaveNum == true){
                waveAmount += 1;
                canAddToWaveNum = false;
            }
        }
        else{
            waveOn = true;
        }
    }

    int GetEnemyCount()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;   
    }
    
}
