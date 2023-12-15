using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;

    public EnemyAttackState(EnemyFSM _manager)
    {
        this.manager = _manager;
        this.parameter = _manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Feelie_FSM_Attack"); 
    }

    public void OnUpdate()
    {
        parameter.info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (parameter.getHurt)
        {
            manager.TransitionState(EnemyStateType.Hurt);
        }

        if (parameter.info.normalizedTime >= .95f)
        {
            manager.TransitionState(EnemyStateType.Chase);
        }
    }

    public void OnExit()
    {

    }
}
