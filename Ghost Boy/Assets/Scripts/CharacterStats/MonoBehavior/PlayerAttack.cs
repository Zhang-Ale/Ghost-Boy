using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine; 

public class PlayerAttack : MonoBehaviour
{
    [Header("Common")]
    public bool isLeft = false;
    public GameObject HitBox;
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
    public float benjiAttackRange = 1f;
    public RuntimeAnimatorController benjiController;
    public CircleCollider2D benjiCol; 

    [Header("Charlie's")]
    public RuntimeAnimatorController charlieController;
    public float charlieAttackRange = 1f;
    public CircleCollider2D charlieCol; 
    public Transform randomAttackPos;
    public GameObject bullet;
    public GameObject bulletPos;
    bool fire;

    private void Awake()
    {
        characterStats = transform.GetComponentInParent<CharacterStats>();
    }

    private void Start()
    {
        characterStats.AttackDamage = 5;
    }

    void Update()
    {
        ChecksToDo();

        if (!isCharlie)
        {
            animator.runtimeAnimatorController = benjiController;
            charlieCol.enabled = false;
            benjiCol.enabled = true;
        }
        else
        {
            animator.runtimeAnimatorController = charlieController;
            benjiCol.enabled = false;
            charlieCol.enabled = true;
        }

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                if (!isCharlie)
                { 
                    BenjaminAttack();
                    nextAttackTime = Time.time + 1f / benjiAttackRate;
                }
                else
                {
                    CharlieAttack();
                    nextAttackTime = Time.time + 1f / charlieAttackRate;
                    StopAllCoroutines();
                    Instantiate(bullet, bulletPos.transform.position, Quaternion.identity);
                    fire = true;
                    StartCoroutine(BulletFire());
                }
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

        if (fire)
        {
            bulletPos.SetActive(true);
        }
        else
        {
            bulletPos.SetActive(false);
        }
    }

    IEnumerator BulletFire()
    {
        yield return new WaitForSeconds(6.5f);
        fire = false;
    }

    void CharlieAttack()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.transform.position, charlieAttackRange, enemyLayers);
        foreach (Collider2D enemyCol in hitEnemies)
        {
            _enemy = enemyCol.transform.gameObject; 

            if (enemyInRange)
            {
                HitBox.transform.position = _enemy.transform.position;
            }
            else
            {
                HitBox.transform.position = randomAttackPos.transform.position;
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
                    enemy.TakeDamage(characterStats.AttackDamage);
                }
            }
        }
    }

    void BenjaminAttack()
    {
        animator.SetTrigger("isAttackOne");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.transform.position, benjiAttackRange, enemyLayers);
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
                    enemy.TakeDamage(characterStats.AttackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (HitBox == null)
            return;
        Gizmos.DrawWireSphere(HitBox.transform.position, benjiAttackRange);
    }
}
