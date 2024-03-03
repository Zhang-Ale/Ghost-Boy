using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFindPlayerState : IEnemyState
{
    private EnemyFSM manager;
    private Parameter parameter;
    
    public EnemyFindPlayerState(EnemyFSM _manager)
    {
        this.manager = _manager;
        this.parameter = _manager.parameter;
    }

    public void OnEnter()
    {
        //should have a startled animation here
    }

    public void OnUpdate()
    {
        parameter.info = parameter.anim.GetCurrentAnimatorStateInfo(0);
        if (parameter.getHurt)
        {
            manager.TransitionState(EnemyStateType.Hurt);
        }
        Debug.Log("Find");
        manager.FlipTo(parameter.target);
        parameter.exclamationMark.SetActive(true);
        parameter.HpBar.SetActive(true);
        //if(info.normalizedTime >= .95f)
        manager.TransitionState(EnemyStateType.Chase); 
    }

    public void OnExit()
    {

    }
}
