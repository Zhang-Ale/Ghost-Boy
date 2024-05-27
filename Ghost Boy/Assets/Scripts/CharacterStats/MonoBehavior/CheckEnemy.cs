using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnemy : MonoBehaviour
{
    public PlayerAttack PA;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyHitbox") || other.gameObject.CompareTag("Breakable"))
        {
            PA.enemyInRange = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EnemyHitbox") || other.gameObject.CompareTag("Breakable"))
        {
            PA.enemyInRange = false;
        }
    }
}
