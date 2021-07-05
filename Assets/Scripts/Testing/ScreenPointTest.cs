using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPointTest : MonoBehaviour
{
  

    [SerializeField]
    static LayerMask mask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // if(Input.GetMouseButtonDown(0)){
        //     setScreenPos();
        // }
    }

    public static void setScreenPos(){
          
            RaycastHit hit;
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit, 1000)){
                Vector2Int pos = new Vector2Int(Mathf.FloorToInt(hit.point.x),Mathf.FloorToInt(hit.point.z));
             //   Debug.Log("Before: " + GridManager.grid.getCellValue(pos.x,pos.y));
                GridManager.grid.setCellValue(pos.x,pos.y);
               // Debug.Log("After: " + GridManager.grid.getCellValue(pos.x,pos.y));
            }
    }
    
}
