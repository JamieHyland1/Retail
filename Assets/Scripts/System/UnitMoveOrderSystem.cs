using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class UnitMoveOrderSystem : ComponentSystem
{

  protected override void OnUpdate(){
      if(Input.GetButtonDown("Jump")){
          Entities.ForEach((Entity entity, ref Translation translation)=>{
            int2 currentPosition = new int2((int)translation.Value.x,(int)translation.Value.z);  
   // ComponentDataFromEntity<PathFindingParams> pathFindingParams = GetComponentDataFromEntity<PathFindingParams>();
            if(!EntityManager.HasComponent<PathFindingParams>(entity)){
                EntityManager.AddComponentData(entity,new PathFindingParams{
                    startPosition = currentPosition,
                    endPosition = GridManager.randomGridPosition()
                });
            }   
      
          
        });
      }
        //    Entities.ForEach((Entity entity,ref Translation translation, ref PathFollow pathPos)=>{
        //      int2 currentPosition = new int2((int)translation.Value.x,(int)translation.Value.z);  
        //       if(pathPos.pathIndex == -1){
        //            EntityManager.AddComponentData(entity,new PathFindingParams{
        //             startPosition = currentPosition,
        //             endPosition = GridManager.randomGridPosition()
        //         });
        //       }
        //  });
         
          }
  }

