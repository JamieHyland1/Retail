using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingState : IState
{

    UnitGameObject unitGameObject;
    List<Vector3> movingList; 
    Transform playerTransform;
    PlayerSM _PlayerSM;


    public MovingState(){

    }

    public MovingState(PlayerSM _PlayerSM, Transform playerTransform, UnitGameObject unitGameObject){
        this._PlayerSM = _PlayerSM;
        this.playerTransform = playerTransform;
        this.unitGameObject = unitGameObject;
    }
    public void Enter()
    {
        movingList = new List<Vector3>();
        unitGameObject.requestPath();
        Debug.Log("Entering moving state!");
    }

    public void Exit()
    {
        Debug.Log("Leaving Moving state");
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {
        movingList = unitGameObject.getMoveList();
        if(movingList.Count > 0){
            Debug.Log("Moving");
            Vector3 targetPos = movingList[0];
            movingList.Remove(targetPos);
            this.playerTransform.position = targetPos;
            if(movingList.Count == 0){
                _PlayerSM.ChangeState(_PlayerSM.idleState);
            }
        }
    }
}
