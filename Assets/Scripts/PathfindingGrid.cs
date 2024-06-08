using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathfindingGrid
{

    private float cellSize;
    private int gridWidth;
    private int gridHeight;
    private float originX;
    private float originY;

    public Dictionary<Vector2,List<Vector2>> pathfindingGraph;

    private Dictionary<Vector2,bool> obstacleDict;

    public PathfindingGrid(int width, int height, float cellSize, float gridOriginX, float gridOriginY){
        this.cellSize = cellSize;
        this.gridHeight = height;
        this.gridWidth = width;

        this.originX = gridOriginX;
        this.originY = gridOriginY;

        this.CalibrateGraph();
        
    }

    //-----PUBLIC METHODS-----------------------------------------------

    public Vector2 GetCellCenter(int x, int y){
        Vector2 cellOrigin = (new Vector2(x,y) * this.cellSize) + new Vector2(this.originX,this.originY);
        return cellOrigin + new Vector2(this.cellSize/2, this.cellSize/2);
    }

    public Vector2 GetCellFromWorldPosition(Vector3 worldPosition){
        int x = Mathf.FloorToInt((worldPosition.x - this.originX)/this.cellSize);
        int y = Mathf.FloorToInt((worldPosition.y - this.originY)/this.cellSize);
        return new Vector2(x,y);
    }

    public Vector2 GetClosestCell(Vector3 worldPosition){
        Vector2 originCell = GetCellFromWorldPosition(worldPosition);
        if (!this.pathfindingGraph.ContainsKey(originCell)){

            List<Vector2> newCells = new List<Vector2>();
            newCells.Add(new Vector2(0,1));
            newCells.Add(new Vector2(0,-1));
            newCells.Add(new Vector2(1,0));
            newCells.Add(new Vector2(-1,0));

            foreach (Vector2 cell in newCells){
                Vector2 newCell = originCell + cell;
                if (this.pathfindingGraph.ContainsKey(newCell) == true){
                    return newCell;
                }
            }
        }
        return originCell;
    }   

    public List<Vector2> GetGraphNeighbours(Vector2 cell){
        return this.pathfindingGraph[cell];
    }

    public void CalibrateGraph(){
        this.pathfindingGraph = new Dictionary<Vector2,List<Vector2>>();
        this.obstacleDict = new Dictionary<Vector2,bool>();
        MakeObstacleDict();
        MakeGraph();
    }

    //---------PRIVATE METHODS-------------------------------------------

    private bool DoesCellContainObstacle(int x, int y){ 
        
        bool containsObstacle = true;

        List<Collider2D> colliders = new List<Collider2D>();
        int colliderNumber = Physics2D.OverlapBox(GetCellCenter(x,y), new Vector2(this.cellSize*0.9f,this.cellSize*0.9f), 0, new ContactFilter2D().NoFilter(), colliders);

        foreach (Collider2D col in colliders){
            if (col.gameObject.tag == "Obstacle"){
                return true;
            }
            if (col.gameObject.tag != "Obstacle"){
                containsObstacle = false;
            }
        }
        
        return containsObstacle;
    }

    private List<Vector2> GetGridNeighbours(int x, int y){
        List<Vector2> neighbours = new List<Vector2>();
        List<Vector2> possibleNeighbours = new List<Vector2>();
        possibleNeighbours.Add(new Vector2(0,1));
        possibleNeighbours.Add(new Vector2(0,-1));
        possibleNeighbours.Add(new Vector2(1,0));
        possibleNeighbours.Add(new Vector2(-1,0));

        possibleNeighbours.Add(new Vector2(-1,1));
        possibleNeighbours.Add(new Vector2(-1,-1));
        possibleNeighbours.Add(new Vector2(1,1));
        possibleNeighbours.Add(new Vector2(1,-1));

        foreach (Vector2 neigh in possibleNeighbours){
            if (!this.obstacleDict[new Vector2(x+(int)neigh.x, y+(int)neigh.y)]){
                neighbours.Add(new Vector2(x+(int)neigh.x, y+(int)neigh.y));
            }
        }
        return neighbours;
    }

    private void MakeGraph(){
        for (int x = 0; x < this.gridWidth; x++){
            for (int y = 0; y < this.gridHeight; y++){
                if (!this.obstacleDict[new Vector2(x,y)]){
                    pathfindingGraph.Add(new Vector2(x,y), GetGridNeighbours(x,y));
                }
            }
        }
    }

    private void MakeObstacleDict(){
        for (int x = 0; x < this.gridWidth; x++){
            for (int y = 0; y < this.gridHeight; y++){
                this.obstacleDict.Add(new Vector2(x,y), DoesCellContainObstacle(x,y));
            }
        }
    }

//---------DEBUGGING FUNCTIONS---------------------------------

    public void DebugFillCell(int x, int y){
        Debug.DrawLine(GetCellCenter(x,y)-new Vector2(this.cellSize/4,0), GetCellCenter(x,y)+new Vector2(this.cellSize/4,0), Color.white, 5f);
        Debug.DrawLine(GetCellCenter(x,y)-new Vector2(0,this.cellSize/4), GetCellCenter(x,y)+new Vector2(0,this.cellSize/4), Color.white, 5f);
    }

    public void DebugObstacleDetection(){
         for (int x = 0; x < this.gridWidth; x++){
            for (int y = 0; y < this.gridHeight; y++){
                if (!DoesCellContainObstacle(x,y)){
                    DebugFillCell(x,y);
                }
            }
        }
    }  

    public void DebugGraph(){
        foreach (KeyValuePair<Vector2,List<Vector2>> entry in this.pathfindingGraph){
            Debug.Log(entry.Key);
            DebugFillCell((int)entry.Key.x, (int)entry.Key.y);
        }
    }


}