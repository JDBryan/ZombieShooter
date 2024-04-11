using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Shockwave : MonoBehaviour
{

    private Material shockwave;
    public float speed;
    private float distance;
    private float maxDistance;
    

    // Start is called before the first frame update
    void Start()
    {
        this.shockwave = this.GetComponent<SpriteRenderer>().material;
        this.distance = -0.1f;
        this.maxDistance = 0.6f;

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        this.distance += Time.deltaTime*this.speed;
        this.shockwave.SetFloat("_WaveDistance", this.distance);
        if (this.distance >= this.maxDistance){
            Destroy(this.gameObject);
        }
    }

    void Update(){
        GameController controller = GameObject.Find("GameController").GetComponent<GameController>();
        if (controller.gameState != GameState.Active){
            Destroy(this.gameObject);
        }
    }

}
