using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private int patrolPosition; 
    public EnemyPatrolState(EnemyFSM _manager)
    {
        this.manager = _manager;
        this.parameter = _manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Feelie_FSM_Walk");
    }

    public void OnUpdate()
    {
        manager.FlipTo(parameter.patrolPoints[patrolPosition]);
        manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.patrolPoints[patrolPosition].position, parameter.moveSpeed * Time.deltaTime);

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

        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patrolPosition].position) < .1f)
        {
            manager.TransitionState(EnemyStateType.Patrol); 
        }

    }

    public void OnExit()
    {
        patrolPosition++; 
        if(patrolPosition >= parameter.patrolPoints.Length)
        {
            patrolPosition = 0; 
        }
    }
}
