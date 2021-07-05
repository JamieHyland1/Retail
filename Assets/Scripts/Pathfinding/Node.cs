using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int x;
    public int y;
    public int gScore;
    public int hScore;
    public int fScore;

    public bool _isWalkable = true;

    public Node parentNode;

    public void setWalkable(){
        this._isWalkable = !_isWalkable;
    }

    public  Node(int x, int y, bool isWall){
        this.x = x;
        this.y = y;
        this._isWalkable = isWall;
    }

    public override string ToString(){
        return this.x + " " + this.y;
    }
    
}
