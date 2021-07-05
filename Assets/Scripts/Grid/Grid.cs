﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid{
    public int width;
    public int height;
    int spacing;
    private Node[,] cells;
    public Grid(){
        //default Constructor;
    }

    public Grid(int width, int height, int spacing){
        this.width = width;
        this.height = height;
        this.spacing = spacing;
        this.cells = new Node[width,height];
    }

    public void generateGrid(){
        for(int j = 0; j < height; j+=spacing){
            for(int i = 0; i < width; i+=spacing){
                cells[i,j] = new Node(i,j,true);
               // Debug.Log(cells[i,j]._isWalkable);
              // if(i == 0 || j == 0 || i == width-1 || j == height-1)cells[i,j].setWalkable();
            }
        }
    }

    public void setCellValue(int i, int j){
        if(i >= 0 && i < width && j >= 0 && j < height){
           // Debug.Log("Setting value at: " + " " + i + " " + j);
            this.cells[i,j].setWalkable();
          //  Debug.Log(this.cells[i,j]._isWalkable);
        }
    }

    public Vector3 getWorldPosition(int x,int y){
        return new Vector3(x,0,y)*spacing;
    }

    public bool getCellValue(int i, int j)
    {
        if(i <0 || i > width || j < 0 || j > height)return false;
        return this.cells[i,j]._isWalkable;
    }
}
