using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSM : StateMachine
{
  
    public MovingState movingState {get; private set;}
    public IdleState idleState {get; private set;}
    [SerializeField]
    UnitGameObject unitGameObject;
    private void Awake() {
        movingState = new MovingState(this,this.transform,unitGameObject);
        idleState = new IdleState(this);
        this.ChangeState(idleState);
    }

    private void Start() {

    }
}
