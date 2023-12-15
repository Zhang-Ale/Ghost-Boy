using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private float idleTimer; 

    public EnemyIdleState(EnemyFSM _manager)
    {
        this.manager = _manager;
        this.parameter = _manager.parameter; 
    }

    public void OnEnter()
    {
        parameter.anim.Play("Feelie_FSM_Idle"); 
    }

    public void OnUpdate()
    {
        idleTimer += Time.deltaTime;

        if (parameter.getHurt)
        {
            manager.TransitionState(EnemyStateType.Hurt);
        }

        if (parameter.target != null &&
           manager.transform.position.x >= parameter.chasePoints[0].position.x ||
           manager.transform.position.x <= parameter.chasePoints[1].position.x)
        {
            manager.TransitionState(EnemyStateType.FindPlayer);
        }

        if (idleTimer >= parameter.idleTime)
        {
            manager.TransitionState(EnemyStateType.Patrol); 
        }
    }

    public void OnExit()
    {
        idleTimer = 0f; 
    }
}
