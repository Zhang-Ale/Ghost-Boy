using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        if(manager.getHurt == true)
        {

        }

    }

    public void OnExit()
    {

    }
    
}
