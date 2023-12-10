using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Common")]
    public bool isLeft = false;
    public GameObject HitBox;
    public Transform LeftSide;
    public Transform RightSide;
    public Animator animator;
    public LayerMask enemyLayers;
    public float attackRate = 1.5f;
    float nextAttackTime = 0f;
    public bool canAttack;
    public bool enemyInRange; 
    private CharacterStats characterStats;
    GameObject _enemy; 
    public GameObject enemyCheckCollider;
    public bool isCharlie = false;

    [Header("Benjamin's")]
    public float attackRange = 1f;
    public RuntimeAnimatorController benjiController;

    [Header("Charlie's")]
    public RuntimeAnimatorController charlieController;
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

        if (Time.time >= nextAttackTime)
        {
            if (Input.GetMouseButtonDown(0) && canAttack)
            {
                if (!isCharlie)
                {
                    animator.runtimeAnimatorController = benjiController;
                    BenjaminAttack();
                    nextAttackTime = Time.time + 1f / attackRate;
                }
                else
                {
                    animator.runtimeAnimatorController = charlieController;
                    if (enemyInRange)
                    {
                        HitBox.transform.position = _enemy.transform.position;
                        CharlieAttack();                        
                    }
                    else
                    {
                        HitBox.transform.position = randomAttackPos.transform.position;
                    }

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
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.transform.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            _enemy = enemy.GetComponent<GameObject>(); 
            if (_enemy.GetComponent<Feelie_Behaviour>() == true)
            {
                _enemy.GetComponent<Feelie_Behaviour>().TakeDamage(characterStats.AttackDamage);
            }

            if (_enemy.GetComponent<TriggerRocks>() == true)
            {
                _enemy.GetComponent<TriggerRocks>().DestroyRock();
            }
            if (_enemy.GetComponent<Enemy>() == true)
            {
                Enemy aenemy = enemy.gameObject.GetComponent<Enemy>();
                if (aenemy != null && aenemy.damageType == DamageTypes.rock)
                {
                    aenemy.TakeDamage(characterStats.AttackDamage);
                }
            }
        }
    }

    void BenjaminAttack()
    {
        animator.SetTrigger("isAttackOne");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(HitBox.transform.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.GetComponent<Feelie_Behaviour>() == true)
            {
                enemy.GetComponent<Feelie_Behaviour>().TakeDamage(characterStats.AttackDamage);
            }

            if (enemy.GetComponent<TriggerRocks>() == true)
            {
                enemy.GetComponent<TriggerRocks>().DestroyRock();
            }
            if (enemy.GetComponent<Enemy>() == true)
            {
                Enemy _enemy = enemy.gameObject.GetComponent<Enemy>();
                if (_enemy != null && _enemy.damageType == DamageTypes.rock)
                {
                    _enemy.TakeDamage(characterStats.AttackDamage);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (HitBox == null)
            return;
        Gizmos.DrawWireSphere(HitBox.transform.position, attackRange);
    }
}
