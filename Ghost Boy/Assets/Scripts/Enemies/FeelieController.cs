using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyStates
{
    GUARD, PATROL, CHASE, DEAD
}
public class FeelieController : MonoBehaviour
{
    private EnemyStates enemyStates;
    [Header("Basic Settings")]
    public float sightRadius; 

    public void Update()
    {
        SwitchStates(); 
    }

    void SwitchStates()
    {
        switch (enemyStates)
        {
            case EnemyStates.GUARD:
                break;
            case EnemyStates.PATROL:
                break;
            case EnemyStates.CHASE:
                break;
            case EnemyStates.DEAD:
                break;
        }
    }
}
