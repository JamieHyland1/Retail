using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using System;

public class MovingState : IState
{

    UnitGameObject unitGameObject;
    
    List<Vector3> movingList; 
    Transform playerTransform;
    PlayerSM _PlayerSM;

    ConvertedEntityHolder convertedEntityHolder;


    public MovingState(){

    }

    public MovingState(PlayerSM _PlayerSM, Transform playerTransform, UnitGameObject unitGameObject, ConvertedEntityHolder convertedEntityHolder){
        this._PlayerSM = _PlayerSM;
        this.playerTransform = playerTransform;
        this.unitGameObject = unitGameObject;
        this.convertedEntityHolder = convertedEntityHolder;
    }
    public void Enter()
    {
        movingList = new List<Vector3>();
        PathFollowSystem.PathRecieved += GetPath;
        unitGameObject.requestPath();
     //   Debug.Log("Entering moving state!");
    }

    public void Exit()
    {
        PathFollowSystem.PathRecieved -= GetPath;
       // Debug.Log("Leaving Moving state");
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {
        if(movingList.Count > 0){
           
                Vector3 pathPosition = movingList[0];
                Vector3 targetPosition = new Vector3(pathPosition.x+0.5f,1f,pathPosition.z+0.5f);
                Vector3 moveDir = Vector3.Normalize(targetPosition-playerTransform.position);

                playerTransform.position += moveDir * 5 * Time.deltaTime;

                if(Vector3.Distance(playerTransform.position, targetPosition) < 0.1f){
                    playerTransform.position = new Vector3(targetPosition.x,1f,targetPosition.z);
                    movingList.Remove(pathPosition);
                }
           
           
           
           
           // Debug.Log("Moving");
            // Vector3 targetPos = movingList[0];
            // movingList.Remove(targetPos);
            // this.playerTransform.position = targetPos;
           
        }else   if(movingList.Count == 0){
            _PlayerSM.ChangeState(_PlayerSM.idleState);
        }
    }


    public void GetPath(object sender, PathFollowSystem.PathRecievedArgs args){
        if(convertedEntityHolder.GetEntity() == args.entity){
            this.movingList = args.path;
            this.movingList.Reverse();
           // foreach(Vector3 pos in args.path)Debug.Log(pos);
        }else {
           // Debug.Log("Not My path");
        }
    }
}
