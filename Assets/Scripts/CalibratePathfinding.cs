using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalibratePathfinding: MonoBehaviour
{
    private Pathfinder pathfinder;
    // Start is called before the first frame update
    void Start()
    {
        this.pathfinder = GameObject.Find("GameController").GetComponent<Pathfinder>();
    }

    public void Calibrate(){
        this.pathfinder.Graph.CalibrateGraph();
    }

    public void SetInactive(){
        this.transform.parent.gameObject.SetActive(false);
    }

}
