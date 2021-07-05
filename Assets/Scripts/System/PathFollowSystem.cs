using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
public class PathFollowSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        Entities.ForEach((DynamicBuffer<PathPosition> pathBuffer, ref Translation translation,ref PathFollow pathFollow)  => {
            if(pathFollow.pathIndex >= 0){
                int2 pathPosition = pathBuffer[pathFollow.pathIndex].position;

                float3 targetPosition = new float3(pathPosition.x+0.5f,0,pathPosition.y+0.5f);
                float3 moveDir = math.normalizesafe(targetPosition-translation.Value);

                translation.Value += moveDir * 5 * UnityEngine.Time.deltaTime;

                if(math.distance(translation.Value, targetPosition) < 0.1f){
                    translation.Value = targetPosition;
                    pathFollow.pathIndex--;
                }
            }
        });
    }
}
