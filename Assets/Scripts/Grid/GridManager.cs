using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class GridManager : MonoBehaviour
{
    [SerializeField,Range(1,100)]
    public static int width = 10;
    [SerializeField,Range(1,100)]
    public static int height = 10;
    
    [SerializeField]
    private LayerMask mask;

    [SerializeField,Range(1,100)]
    int spacing = 5;
    public static Grid grid;
   
    void Start(){
        if(grid == null){
//            Debug.Log("Initializing grid: " + Time.realtimeSinceStartup);
            grid = new Grid(width,height,spacing);
            grid.generateGrid();
            checkGridLayout();
        }
    }

    public Vector2Int mouseToWorldPos(){
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-Camera.main.transform.position.z));
        worldPosition.y = 0;
        Vector2Int val = new Vector2Int(Mathf.FloorToInt(worldPosition.x),Mathf.FloorToInt(worldPosition.z));
        return val;
    }

    public static int2 randomGridPosition(){
        int x = Mathf.FloorToInt(UnityEngine.Random.RandomRange(0,grid.width));
        int y = Mathf.FloorToInt(UnityEngine.Random.RandomRange(0,grid.height));
        return new int2(x,y);
    }

    private void checkGridLayout(){
        RaycastHit hit;
        for(int j = 0; j < height; j++){
            for(int i = 0; i < width; i ++){
                Vector3 worldPos = grid.getWorldPosition(i,j);
                worldPos+=(Vector3.up*2.5f);
                worldPos+=(Vector3.right*0.5f);
               // Debug.Log(worldPos);
               Debug.DrawRay(worldPos,Vector3.down,Color.red);
                if (Physics.Raycast(worldPos, Vector3.down, out hit, Mathf.Infinity, mask)){
                    if(hit.collider != null){
                        grid.setCellValue(i,j);
                    }
                }
            }
        }
    }


    private void OnDrawGizmos() {
        if(Application.isPlaying){
            for(int j = 0; j < height; j++){
                for(int i = 0; i < width; i++){
                   // if(grid.getCellValue(i,j) == 0){
                        Gizmos.color = Color.red;
                        Gizmos.DrawLine(grid.getWorldPosition(i,j), grid.getWorldPosition(i+1,j));
                        Gizmos.DrawLine(grid.getWorldPosition(i,j), grid.getWorldPosition(i,j+1));
                        //Debug.Log(grid.getCellValue(i,j));
                        if(!grid.getCellValue(i,j) == true)Gizmos.color = Color.green; else Gizmos.color = Color.yellow;
                        Gizmos.DrawSphere(grid.getWorldPosition(i,j)+new Vector3(spacing,0,spacing)*0.5f ,0.1f);
                        
                    //}
                }
            }
            Gizmos.color = Color.white;
            Gizmos.DrawLine(grid.getWorldPosition(0,height), grid.getWorldPosition(width,height));
            Gizmos.DrawLine(grid.getWorldPosition(width,height), grid.getWorldPosition(width,0));
        }
    }
}
