using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHurtState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;

    public EnemyHurtState(EnemyFSM _manager)
    {
        this.manager = _manager;
        this.parameter = _manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Feelie_FSM_Hurt");
    }

    public void OnUpdate()
    {
        manager.FlashColor(0.2f);
        parameter.info = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if(parameter.characterStats.CurHealth <= 0)
        {
            manager.TransitionState(EnemyStateType.Die);
        }
        else
        {
            if(parameter.info.normalizedTime >= .95f)
            {
                parameter.target = GameObject.FindGameObjectWithTag("Player").transform;
                manager.TransitionState(EnemyStateType.Chase);
            }
        }
    }
    

    public void OnExit()
    {
        parameter.getHurt = false; 
    }
    
}
