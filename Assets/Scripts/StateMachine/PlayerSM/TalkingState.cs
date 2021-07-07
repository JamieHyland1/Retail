using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingState : IState
{
    PlayerSM _playerSm;
    

    public TalkingState(PlayerSM _playerSM){
        this._playerSm = _playerSM;
    }
    public void Enter(){
    }

    public void Exit(){
    }

    public void FixedTick(){
    }

    public void Tick(){
    }
}
