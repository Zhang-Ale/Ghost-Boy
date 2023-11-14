using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public Transform attackHitBox;
    public Animator animator;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public int attackDamage = 10;
    public float attackRate = 1.5f;
    float nextAttackTime = 0f;
    public bool canAttack;

    private void Start()
    {
        canAttack = false;
    }

    void Update()
    {
        if(Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                Attack();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
    }  

    void Attack()
    { 
         animator.SetTrigger("isAttackOne");
         Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
         foreach (Collider2D enemy in hitEnemies)
         {      
            if (enemy.GetComponent<Feelie_Behaviour>() == true)
            {
                enemy.GetComponent<Feelie_Behaviour>().TakeDamage(attackDamage);
            }

            if (enemy.GetComponent<TriggerRocks>() == true)
            {
                enemy.GetComponent<TriggerRocks>().DestroyRock();
            }
         }      
    }

    private void OnDrawGizmosSelected()
    {
        if (attackHitBox == null)
            return;
        Gizmos.DrawWireSphere(attackHitBox.position, attackRange);
    }
}
