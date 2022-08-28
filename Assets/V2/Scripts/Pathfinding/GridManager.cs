using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour{

    [Header("Cell Info")]
    public LayerMask layerMask;
    public Vector2Int gridDim;
    public float unitLength;
    public Cell[,] grid;
    //public Transform targetPos;
    public List<Cell> openSet = new List<Cell>();
    public List<Cell> closedSet = new List<Cell>();
    public Vector3 offset;

/* CELL MANAGER */
    //Setup the grid
    public void Awake(){

        //This variable is used later when spawning the stage to have the center of the grid be at Vector.zero
        offset = new Vector3(gridDim.x,0,gridDim.y) * 0.5f * unitLength;
        grid = new Cell[gridDim.x,gridDim.y];

        //Loop through the grid and create a Cell class at every position
        for(int x = 0;x < gridDim.x;x++){

            for(int y = 0;y < gridDim.y;y++){

                //When translating the grid position to world space set the center of the grid to (0,0) by shifting it by an offset
                Vector3 pos = new Vector3(x * unitLength + (unitLength/2) ,0,y * unitLength + (unitLength/2) ) - offset;
                //Check if there is an obstacle at this cells position, if so then set the cell as not walkable
                bool isWalkable = !(Physics.CheckSphere(pos,unitLength/2f,layerMask));

                grid[x,y] = new Cell(isWalkable,pos,new Vector2Int(x,y));

            }

        }

    }

    //Translate a world position to the nearest cell in the grid
    public Cell WorldPosToCell(Vector3 pos){

        pos += offset;
        pos /= unitLength;
        pos = new Vector3(Mathf.Clamp(pos.x,0,gridDim.x),0,Mathf.Clamp(pos.z,0,gridDim.y));
        Vector3Int posN = Vector3Int.RoundToInt(pos); 

        return grid[posN.x,posN.z];

    }

    public List<Cell> GetNeighbours(Cell n){

        List<Cell> neighbours = new List<Cell>();

        //We are checking the 8 neighbouring cells so we look in a 3x3 area around the original cell
        for(int x = -1;x < 2;x++){

            for(int y = -1;y < 2;y++){

                //If referencing the original cells position
                if(x == 0 && y == 0){

                    continue;

                }

                //Neighbours grid position
                int neighX = n.gridPos.x + x;
                int neighY = n.gridPos.y + y;

                //If the neighbours position is within the grid
                if(neighX >= 0 && neighY >= 0 && neighX < gridDim.x && neighY < gridDim.y){

                    neighbours.Add(grid[neighX,neighY]);

                }

            }

        }

        return neighbours;

    }
//G cost the distance that cell is from the starting node
//H cost is the distance that cell is from the ending node
//F cost is the sum of G cost and H cost
/* PATHFINDING */
    public int CalculateCost(Vector3 s, Vector3 e){

        //Clear the previous frame's data
        openSet.Clear();
        closedSet.Clear();

        //Set the start and end of the path
        Cell start = WorldPosToCell(s);
        Cell end = WorldPosToCell(e);

        //Add the start to your open set
        openSet.Add(start);

        //Loop until we've cleared our open set
        while(openSet.Count > 0){

            //Set the current cell to the first cell in the set
            Cell currentCell = openSet[0];

            for(int i = 1; i < openSet.Count;i++){

                //If the open set and closed set have the same F cost then compare their H costs instead (distance to target Cell)
                if(openSet[i].F() < currentCell.F() 
                || (openSet[i].F() == currentCell.F() && openSet[i].h < currentCell.h)){

                                
                    currentCell = openSet[i];

                }

            }

            //Remove the cell from the open set and add it to our closed set
            openSet.Remove(currentCell);
            closedSet.Add(currentCell);

            if(currentCell == end){

                //We've reached our target so retrace our steps and tell the car to start moving towards its target
                //Retrace(start,end);
                return currentCell.F();

            }

            //Look through the current cells neighbouring cells and set their G cost, H cost and its parent cell
            foreach(Cell neigh in GetNeighbours(currentCell)){

                if(!neigh.walkable || closedSet.Contains(neigh)){

                    continue;

                }

                //Calculate the path distance from the start to the neighbour
                int disToNeighbour = currentCell.g + GetDis(currentCell,neigh);

                //If the G cost of the neighbouring cell is inaccurate then update the cell
                if(disToNeighbour < neigh.g || !openSet.Contains(neigh)){

                    neigh.g = disToNeighbour;
                    neigh.h = GetDis(neigh,end);
                    neigh.parent = currentCell;

                    if(!openSet.Contains(neigh)){

                        //Add the neighbouring cell to the open set
                        openSet.Add(neigh);

                    }

                }

            }

        }

        return -1;

    }

    void Retrace(Cell start,Cell end){

        List<Cell> path = new List<Cell>();

        //Start at the end of the path
        Cell current = end;

        //Loop backwards
        while(current != start){
            path.Add(current);
            //Spawn a target at the Cells position

            //Set the current Cell to its parent Cell
            current = current.parent;

        }

        //Reverse the path so that it goes start to end
        path.Reverse();

    }

    int GetDis(Cell a, Cell b){

        int x = Mathf.Abs(a.gridPos.x - b.gridPos.x);
        int y = Mathf.Abs(a.gridPos.y - b.gridPos.y);

        if(x > y){

            return 14 * y + 10 * (x-y);

        } else {

            return 14 * x + 10 * (y-x);

        }
         
    }

    void OnDrawGizmos(){

        // //Draw a cube showing the pathfinding space
        // Gizmos.DrawWireCube(transform.position,new Vector3(gridDim.x,1,gridDim.y) * unitLength);

        // if(grid != null){

        //     //Look through every cell and visually show if it is walkable or not
        //     foreach(Cell cell in grid){

        //         if(cell.walkable){

        //             Gizmos.color = Color.green;

        //         } else {

        //             Gizmos.color = Color.red;

        //         }

        //         Gizmos.DrawCube(cell.pos,Vector3.one * unitLength * 0.9f);

        //     }

        // }

    }

}
