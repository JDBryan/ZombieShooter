using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinder : MonoBehaviour
{
    public PathfindingGrid Graph;
    private Vector3 goal;
    private Vector3 start;
    //  cameFrom contains keys that are grid cell coordinates, and values
    //  that are the grid cell coordinates that generated the key in the BreadthFirstSearch.
    //  So to find the cell B that grid cell A points to:   B = cameFrom[A]
    private Dictionary<Vector2,Vector2> cameFrom;
    private Vector2 lastValidCell;

    // Start is called before the first frame update
    void Start()
    {
        this.Graph = new PathfindingGrid(200,170,0.5f,-50f,-10f);
        this.start = new Vector3(0f,0f,0f);
        this.goal = GameObject.FindGameObjectWithTag("Player").transform.position;
        this.cameFrom = new Dictionary<Vector2,Vector2>();
        this.lastValidCell = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        this.goal = GameObject.FindGameObjectWithTag("Player").transform.position;
        Vector2 goalCell = Graph.GetClosestCell(this.goal);
        if (Graph.pathfindingGraph.ContainsKey(goalCell)){
            this.lastValidCell = goalCell;
        }
        this.cameFrom = BreadthFirstSearch(this.lastValidCell);
    }

    public void Reset(){
        this.Graph = new PathfindingGrid(200,170,0.5f,-50f,-10f);
    }

    private Dictionary<Vector2,Vector2> BreadthFirstSearch(Vector2 target){
        this.cameFrom.Clear();
        Queue<Vector2> frontier = new Queue<Vector2>();
        frontier.Enqueue(target);
        this.cameFrom.Add(target, new Vector2());

        while (frontier.Count != 0){
            Vector2 current = frontier.Dequeue();
            foreach (Vector2 neighbour in this.Graph.GetGraphNeighbours(current)){
                if (!cameFrom.ContainsKey(neighbour)){
                    frontier.Enqueue(neighbour);
                    cameFrom.Add(neighbour, current);
                }
            }
        } 
        return cameFrom;
    }

    public Vector2 NextDestination(Vector3 startPosition){
        Vector2 currentCell = Graph.GetClosestCell(startPosition);
        Vector2 nextCell = this.cameFrom[currentCell];
        return Graph.GetCellCenter((int)nextCell.x, (int)nextCell.y);
    }

    public bool CheckIfCellInCameFrom(Vector3 position){
        return this.cameFrom.ContainsKey(Graph.GetClosestCell(position));
    }

    private void DebugPath(Vector3 position){
        Vector2 goal = this.lastValidCell;
        Vector2 currentCell = Graph.GetClosestCell(position);
        while (currentCell != goal){
            Vector2 nextCell = this.cameFrom[currentCell];
            Debug.DrawLine(Graph.GetCellCenter((int)currentCell.x,(int)currentCell.y), Graph.GetCellCenter((int)nextCell.x,(int)nextCell.y), Color.white);
            currentCell = nextCell;
        }    
    }
}
