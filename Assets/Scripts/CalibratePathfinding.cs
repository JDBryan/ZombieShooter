using UnityEngine;

public class CalibratePathfinding: MonoBehaviour
{
    public void Calibrate()
    {
        Pathfinder.Instance.Graph.CalibrateGraph();
    }

    public void SetInactive(){
        this.transform.parent.gameObject.SetActive(false);
    }

}
