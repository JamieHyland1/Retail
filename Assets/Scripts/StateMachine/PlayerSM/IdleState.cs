using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    PlayerSM _PlayerSM;
    float waitTime = 5;
    float counter;

    Transform transform;

    public IdleState(){

    }
    public IdleState(PlayerSM _PlayerSM, Transform transform){
        this._PlayerSM = _PlayerSM;
        this.waitTime = Random.Range(1f,7f);
        this.transform = transform;
    }

    public void Enter()
    {
        //Debug.Log("Entering Idle state");
        GridManager.grid.setCellValue((int)transform.position.x,(int)transform.position.z);
        counter = waitTime;
    }

    public void Exit()
    {
      //  Debug.Log("Leaving Idle state");
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {
        counter -= Time.deltaTime;
        if(counter <= 0){
            _PlayerSM.ChangeState(_PlayerSM.movingState);
            GridManager.grid.setCellValue((int)transform.position.x,(int)transform.position.z);
        }
    }
}
