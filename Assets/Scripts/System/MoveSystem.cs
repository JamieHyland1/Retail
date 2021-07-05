using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public class MoveSystem : ComponentSystem
{
  
    protected override void OnUpdate()
    {
        Entities.ForEach((ref Translation transform, ref MoveSpeedComponent mSpeed) => {
       //   transform.Value.y += Mathf.Sin(Time.deltaTime) * mSpeed.moveSpeed * Time.deltaTime;
      });
    }
}
