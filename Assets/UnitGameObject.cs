using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
public class UnitGameObject : MonoBehaviour
{
    [SerializeField]
    private ConvertedEntityHolder convertedEntityHolder;

    void Start()
    {
//        Debug.Log(convertedEntityHolder.GetEntity());
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        Entity entity = convertedEntityHolder.GetEntity();
        EntityManager manager = convertedEntityHolder.GetEntityManager();
        if(Input.GetMouseButtonDown(0)){
            manager.AddComponentData(entity, new PathFindingParams{
                startPosition = new int2(0,0),
                endPosition = GridManager.randomGridPosition()
            });
        }
        PathFollow pathFollow = manager.GetComponentData<PathFollow>(entity);
        DynamicBuffer<PathPosition> pathBuffer  = manager.GetBuffer<PathPosition>(entity);
         if(pathFollow.pathIndex >= 0){
                int2 pathPosition = pathBuffer[pathFollow.pathIndex].position;

                float3 targetPosition = new float3(pathPosition.x+0.5f,0,pathPosition.y+0.5f);
                float3 moveDir = math.normalizesafe(targetPosition-(float3)transform.position);

                transform.position += (Vector3)moveDir * 5 * Time.deltaTime;

                if(math.distance((float3)transform.position, targetPosition) < 0.1f){
                    transform.position = targetPosition;
                    pathFollow.pathIndex--;
                }
                //transform.position = pos;
            }
    }
}
