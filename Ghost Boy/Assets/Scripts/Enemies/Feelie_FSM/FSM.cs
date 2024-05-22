using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    Idle, Patrol, Chase, React, Attack, Hurt, Die
}

public class FSM : Enemy
{
    private BaseState currentState;

    void Start()
    {
        currentSpeed = normalSpeed;
    }

    public override void Move()
    {
        base.Move();
    }
}
