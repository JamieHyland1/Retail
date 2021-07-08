using System;
using System.Collections.Generic;

using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
using Unity.Transforms;
public class PathFollowSystem : ComponentSystem
{
     public static event EventHandler<PathRecievedArgs> PathRecieved; 

     public class PathRecievedArgs : EventArgs{
        public List<Vector3> path;
        public Entity entity;
     }

    protected override void OnUpdate()
    {
        Entities.ForEach((Entity entity, DynamicBuffer<PathPosition> pathBuffer, ref Translation translation,ref PathFollow pathFollow)  => {
            if(pathFollow.pathIndex >= 0){
                // int2 pathPosition = pathBuffer[pathFollow.pathIndex].position;

                // float3 targetPosition = new float3(pathPosition.x+0.5f,0,pathPosition.y+0.5f);
                // float3 moveDir = math.normalizesafe(targetPosition-translation.Value);

                // translation.Value += moveDir * 5 * UnityEngine.Time.deltaTime;

                // if(math.distance(translation.Value, targetPosition) < 0.1f){
                //     translation.Value = targetPosition;
                //     pathFollow.pathIndex--;
                // }
                List<Vector3> pth = new List<Vector3>();
                for(int i = 0; i < pathBuffer.Length; i++){
                    pth.Add(new Vector3(pathBuffer[i].position.x,1,pathBuffer[i].position.y));
                }
                PathRecieved?.Invoke(this,new PathRecievedArgs{path = pth, entity = entity});
                pathFollow.pathIndex = -1;
            }
        });
    }
}
