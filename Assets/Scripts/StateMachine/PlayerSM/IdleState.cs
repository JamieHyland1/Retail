using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{

    PlayerSM _PlayerSM;
    float waitTime = 5;
    float counter;

    public IdleState(){

    }
    public IdleState(PlayerSM _PlayerSM){
        this._PlayerSM = _PlayerSM;
    }

    public void Enter()
    {
        Debug.Log("Entering Idle state");
        counter = waitTime;
    }

    public void Exit()
    {
        Debug.Log("Leaving Idle state");
    }

    public void FixedTick()
    {

    }

    public void Tick()
    {
        counter -= Time.deltaTime;
        if(counter <= 0){
            _PlayerSM.ChangeState(_PlayerSM.movingState);
        }
    }
}
