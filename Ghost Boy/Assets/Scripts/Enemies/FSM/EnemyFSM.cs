using System; 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public enum EnemyStateType
{
    Idle, Patrol, FindPlayer, Chase, Attack, Hurt, Die
}

[Serializable]
public class Parameter
{
    public CharacterStats characterStats;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime; 
    public Transform[] patrolPoints;
    //public Collider2D triggeredRange; 
    public Transform[] chasePoints;
    public Transform target; 
    public LayerMask targetLayer;
    public Transform attackPoint;
    public float attackArea; 
    public Animator anim, lightAnim;
    public GameObject exclamationMark, HpBar;
    public AnimatorStateInfo info;
    public SpriteRenderer SR;
    public Color originalColor;
    public Light2D blinkLight;
    public GameObject bloodEffect;
}

public class EnemyFSM : MonoBehaviour
{
    public Parameter parameter; 
    private IEnemyState currentState;
    private Dictionary<EnemyStateType, IEnemyState> states = new Dictionary<EnemyStateType, IEnemyState>();
    public bool getHurt;

    void Start()
    {
        parameter.originalColor = parameter.SR.color;
        parameter.anim = GetComponent<Animator>();
        parameter.lightAnim = transform.GetChild(3).GetComponent<Animator>(); 
        parameter.characterStats = GetComponentInParent<CharacterStats>(); 
        parameter.SR = GetComponent<SpriteRenderer>();
        states.Add(EnemyStateType.Idle, new EnemyIdleState(this));
        states.Add(EnemyStateType.Patrol, new EnemyPatrolState(this));
        states.Add(EnemyStateType.FindPlayer, new EnemyFindPlayerState(this));
        states.Add(EnemyStateType.Chase, new EnemyChaseState(this));
        states.Add(EnemyStateType.Attack, new EnemyAttackState(this));
        states.Add(EnemyStateType.Hurt, new EnemyHurtState(this));
        states.Add(EnemyStateType.Die, new EnemyDieState(this));

        TransitionState(EnemyStateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void TransitionState(EnemyStateType type)
    {
        if(currentState != null)
        {
            currentState.OnExit(); 
        }
        currentState = states[type];
        currentState.OnEnter(); 
    }

    public void TakeDamage(int damage)
    {
        parameter.characterStats.CurHealth = Mathf.Max(parameter.characterStats.CurHealth - damage, 0);
        FlashColor(0.2f);
        Instantiate(parameter.bloodEffect, parameter.blinkLight.transform.position, Quaternion.identity);
        parameter.anim.SetTrigger("Damaged");
    }

    private void FlashColor(float time)
    {
        parameter.SR.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        parameter.SR.color = parameter.originalColor;
    }

    public void FlipTo(Transform target)
    {
        if(target != null)
        {
            if(transform.position.x > target.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }else if(transform.position.x < target.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1); 
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            parameter.target = collision.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(parameter.attackPoint.position, parameter.attackArea); 
    }
}
