using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class LevelUpSystem : ComponentSystem
{
  protected override void OnUpdate(){
      Entities.ForEach((ref LevelComponent LevelComponent) => {
      //    LevelComponent.level += 1 * Time.deltaTime;
          //Debug.Log(LevelComponent.level);
      });
  }
}
