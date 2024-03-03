using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyChaseState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;

    public EnemyChaseState(EnemyFSM _manager)
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
        manager.FlipTo(parameter.target);
        if (parameter.target)
        {
            manager.transform.position = Vector2.MoveTowards(manager.transform.position,
            parameter.target.position, parameter.chaseSpeed * Time.deltaTime);
            parameter.lightAnim.SetBool("ifInRange", true);
        }

        if (parameter.getHurt)
        {
            manager.TransitionState(EnemyStateType.Hurt);
        }

        if (parameter.target == null ||
            manager.transform.position.x < parameter.chasePoints[0].position.x ||
            manager.transform.position.x > parameter.chasePoints[1].position.x)
        {
            Debug.Log("Chase");
            manager.TransitionState(EnemyStateType.Idle);
            parameter.lightAnim.SetBool("ifInRange", false);
            parameter.exclamationMark.SetActive(false);
            parameter.HpBar.SetActive(false);
        }    
        
        if(Physics2D.OverlapCircle(parameter.attackPoint.position, parameter.attackArea, parameter.targetLayer))
        {
            manager.TransitionState(EnemyStateType.Attack); 
        }
        
    }

    public void OnExit()
    {

    }
}
