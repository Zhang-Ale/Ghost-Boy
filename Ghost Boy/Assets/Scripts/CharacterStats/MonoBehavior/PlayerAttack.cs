using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Common")]
    public bool isLeft = false;
    public Transform LeftSide;
    public Transform RightSide;
    public Animator animator;
    public LayerMask enemyLayers;
    public float benjiAttackRate = 1.25f;
    public float charlieAttackRate = 1.8f;
    float nextAttackTime = 0f;
    public bool canAttack;
    public bool enemyInRange; 
    private CharacterStats characterStats;
    GameObject _enemy; 
    public GameObject enemyCheckCollider;
    public bool isCharlie = false;

    [Header("Benjamin's")]
    public int benjiAttackDamage; 
    public GameObject benjiHitBox;
    public float benjiAttackRange = 1f;
    public RuntimeAnimatorController benjiController;
    public CircleCollider2D benjiCol;

    [Header("Charlie's")]
    public int charlieAttackDamage; 
    public GameObject charlieHitBox;
    public RuntimeAnimatorController charlieController;
    public float charlieAttackRange = 1f;
    public CircleCollider2D charlieCol;
    public BulletPool bulletPool;
    public Transform attackPos;
    public GameObject bullet;
    public GameObject bulletPos;
    public bool wait;
    float waitTimeCounter;
    public float waitTime; 

    private void Awake()
    {
        characterStats = transform.GetComponentInParent<CharacterStats>();
    }

    private void Start()
    {
        characterStats.AttackDamage = benjiAttackDamage;
        charlieHitBox.transform.position = attackPos.transform.position;
        waitTimeCounter = waitTime;
    }

    void Update()
    {
        ChecksToDo();
        TimeCounter();

        if (!isCharlie)
        {
            animator.runtimeAnimatorController = benjiController;
            characterStats.AttackDamage = benjiAttackDamage;
            charlieCol.enabled = false;
            benjiCol.enabled = true;
        }
        else
        {
            animator.runtimeAnimatorController = charlieController;
            characterStats.AttackDamage = charlieAttackDamage;
            benjiCol.enabled = false;
            charlieCol.enabled = true;
        }

        if (Input.GetMouseButtonDown(0) && canAttack)
        {
            if (!isCharlie)
            {
                if (Time.time >= nextAttackTime)
                {
                    BenjaminAttack();
                    nextAttackTime = Time.time + 1f / benjiAttackRate;
                }
            }
            else if(isCharlie && !wait)
            {
                CharlieAttack();
                wait = true;
                bulletPool.GetBullet(); 
            }
        }
    }

    void ChecksToDo()
    {
        if (Input.GetKeyDown(KeyCode.A) && !isLeft)
        {
            isLeft = true;
        }
        if (Input.GetKeyDown(KeyCode.D) && isLeft)
        {
            isLeft = false;
        }
    }

    void TimeCounter()
    {
        if (wait)
        {
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                bulletPool.ReturnBullet(); 
                waitTimeCounter = waitTime;
            }
        }
    }

    void CharlieAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(charlieHitBox.transform.position, charlieAttackRange, enemyLayers);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            _enemy = enemyCol.transform.gameObject; 

            if (enemyInRange)
            {
                charlieHitBox.transform.position = _enemy.transform.position;
            }
            else
            {
                charlieHitBox.transform.position = attackPos.transform.position;
            }

            if (_enemy.GetComponent<EnemyFSM>() == true)
            {
                _enemy.GetComponent<EnemyFSM>().TakeDamage(characterStats.AttackDamage); 
            }

            if (_enemy.GetComponent<TriggerRocks>() == true)
            {
                _enemy.GetComponent<TriggerRocks>().DestroyRock();
            }
            if (_enemy.GetComponent<Enemy>() == true)
            {
                Enemy enemy = enemyCol.gameObject.GetComponent<Enemy>();
                if (enemy != null && enemy.damageType == DamageTypes.Tunk || enemy.damageType == DamageTypes.Feelie)
                {
                    enemy.OnTakeDamage(characterStats.AttackDamage, this.transform);
                }
            }
        }
    }

    void BenjaminAttack()
    {
        animator.SetTrigger("isAttackOne");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(benjiHitBox.transform.position, benjiAttackRange, enemyLayers);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            _enemy = enemyCol.transform.gameObject;
            if (_enemy.GetComponent<EnemyFSM>() == true)
            {
                _enemy.GetComponent<EnemyFSM>().TakeDamage(characterStats.AttackDamage);
            }

            if (_enemy.GetComponent<TriggerRocks>() == true)
            {
                _enemy.GetComponent<TriggerRocks>().DestroyRock();
            }
            if (_enemy.GetComponent<Enemy>() == true)
            {
                Enemy enemy = enemyCol.gameObject.GetComponent<Enemy>();
                if (enemy != null && enemy.damageType == DamageTypes.Tunk || enemy.damageType == DamageTypes.Feelie)
                {
                    enemy.OnTakeDamage(characterStats.AttackDamage, this.transform);
                }
            }
        }
    }
}
