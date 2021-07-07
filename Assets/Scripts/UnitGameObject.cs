using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class UnitGameObject : MonoBehaviour
{
    [SerializeField]
    private ConvertedEntityHolder convertedEntityHolder;
    List<Vector3> moveList;

    MovingState movingState;
    void Start()
    {
        // if(convertedEntityHolder == null){
        //     convertedEntityHolder = this.GetComponent<ConvertedEntityHolder>();
        //     Debug.Log("null");
        // }
        moveList = new List<Vector3>();
    }

   
    void Update()
    {
        Vector3 pos = transform.position;
        Entity entity = convertedEntityHolder.GetEntity();
        EntityManager manager = convertedEntityHolder.GetEntityManager();
        // if(Input.GetMouseButtonDown(0)){
        //     manager.AddComponentData(entity, new PathFindingParams{
        //         startPosition = new int2(0,0),
        //         endPosition = GridManager.randomGridPosition()
        //     });
        // }
        PathFollow pathFollow = manager.GetComponentData<PathFollow>(entity);
        DynamicBuffer<PathPosition> pathBuffer  = manager.GetBuffer<PathPosition>(entity);
        if(pathFollow.pathIndex >= 0){
            Vector3 currentPos = new Vector3(pathBuffer[pathFollow.pathIndex].position.x,1,pathBuffer[pathFollow.pathIndex].position.y);
            moveList.Add(currentPos);    
            if(pathFollow.pathIndex > 0)pathFollow.pathIndex--;
        }
    }

    public List<Vector3> getMoveList(){
        PathFollow pathFollow = convertedEntityHolder.GetEntityManager().GetComponentData<PathFollow>(convertedEntityHolder.GetEntity());
        pathFollow.pathIndex--;
        return moveList;
    }

    public void requestPath(){
        Entity entity = convertedEntityHolder.GetEntity();
        EntityManager manager = convertedEntityHolder.GetEntityManager();
        Debug.Log(manager);
         PathFollow pathPos = manager.GetComponentData<PathFollow>(entity);
        int2 currentPosition = new int2((int)this.transform.position.x,(int)this.transform.position.z);  
        if(pathPos.pathIndex == -1){
            manager.AddComponentData(entity,new PathFindingParams{
                startPosition = currentPosition,
                endPosition = GridManager.randomGridPosition()
            });
        }
    }
}
