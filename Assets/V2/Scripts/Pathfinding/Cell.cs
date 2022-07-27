using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell {

    public bool walkable;
    public Vector3 pos;
    public Vector2Int gridPos;
    public int g;
    public int h;
    public Cell parent;

    public Cell(bool w,Vector3 p,Vector2Int gPos){

        walkable = w;
        pos = p;
        gridPos = gPos;

    }

    public int F(){

        return g + h;

    }

}