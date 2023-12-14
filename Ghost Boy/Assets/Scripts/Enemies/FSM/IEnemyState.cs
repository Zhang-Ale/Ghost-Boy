using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyState 
{
    void OnEnter();

    void OnUpdate();

    void OnExit(); 
}
