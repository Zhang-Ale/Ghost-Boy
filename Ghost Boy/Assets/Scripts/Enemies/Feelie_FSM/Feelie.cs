using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Feelie : Enemy
{
    #region Public Variables
    public float attackDistance;
    public float timer;
    public bool inRange; //check if player is in range
    public Transform player;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    public float playerDistance; //distance between this & player
    public GameObject hotZone;
    public GameObject triggerArea;
    public AudioClip BattleMusic;
    public AudioClip BackMusic;
    public Transform attackHitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public Light2D blinkLight;
    public GameObject bloodEffect;
    public FeelieHpBar HealthBar;
    public bool flipped = false;
    public bool critHit;
    #endregion

    #region Private Variables
    private bool coroutineStarted = false;
    private float distance; //distance between this & target
    SpriteRenderer SR;
    private Color originalColor;
    [SerializeField]
    bool attackMode;
    [SerializeField]
    bool FeelieIsDamaged;
    [SerializeField]
    bool playerIsDamaged;
    [SerializeField]
    private bool cooling; //check is this is cooling after attack
    private float intTimer;
    #endregion

    private void Start()
    {
        damageType = DamageTypes.Feelie;
        SelectTheTarget();
        intTimer = timer; //to store the initial value of timer
        SR = GetComponent<SpriteRenderer>();
        originalColor = SR.color;
        curHealth = maxHealth; 
        HealthBar.SetHealth(curHealth, maxHealth);
        flipped = false;
    }

    void FixedUpdate()
    {
        HealthBar.SetHealth(curHealth, maxHealth);
        if (player != null)
        {
            playerDistance = Vector2.Distance(transform.position, player.position);
        }
        if (!FeelieIsDamaged && !attackMode)
        {
            Move();
            if (!coroutineStarted)
            {
                StartCoroutine(IdleAndMove());
            }
        }
        else
        {
            StopCoroutine(IdleAndMove());
        }

        if (!InsideofLimits() && !inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("Feelie_attack"))
        {
            SelectTheTarget();
        }

        if (inRange && !cooling)
        {
            StartCoroutine(EnemyLogic());
        }

        if (curHealth <= 0)
        {
            Die();
        }

        if (playerDistance >= 35)
        {
            maxHealth = 100;
            playerDistance = 0;
        }
    }

    IEnumerator IdleAndMove()
    {
        coroutineStarted = true;
        if (!inRange)
        {
            currentSpeed = normalSpeed; 
            yield return new WaitForSeconds(5);
            currentSpeed = 0f; 
            yield return new WaitForSeconds(5);
            coroutineStarted = false;
        }
    }

    IEnumerator ControlMove()
    {
        FeelieIsDamaged = true;
        Debug.Log("started");
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        cooling = false;
        FeelieIsDamaged = false;
        Debug.Log("ended");
    }

    public override void TakeDamage(int damage)
    {
        StartCoroutine(ControlMove());
        curHealth = Mathf.Max(curHealth - damage, 0);
        FlashColor(0.2f);
        Instantiate(bloodEffect, blinkLight.transform.position, Quaternion.identity);
        anim.SetTrigger("Damaged");
    }

    private void FlashColor(float time)
    {
        SR.color = Color.red;
        Invoke("ResetColor", time);
    }

    private void ResetColor()
    {
        SR.color = originalColor;
    }

    void Die()
    {
        Animator lightAnim = blinkLight.GetComponent<Animator>();
        lightAnim.SetBool("ifInRange", false);
        anim.SetBool("Dead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(this.gameObject, 2f);
        // if(BackMusic != null)
        // {
        //     music.ChangeBGM(BackMusic);
        // }       
    }

    IEnumerator EnemyLogic()
    {
        currentSpeed = chaseSpeed; 
        Animator lightAnim = blinkLight.GetComponent<Animator>();
        lightAnim.SetBool("ifInRange", true);
        distance = Vector2.Distance(transform.position, target.position);

        if (attackDistance > distance && !playerIsDamaged && !cooling && !FeelieIsDamaged)
        {
            playerIsDamaged = true;
            Attack();
            yield return new WaitForSeconds(0.75f);
            playerIsDamaged = false;
        }

        if (cooling)
        {
            Cooldown();
            anim.SetBool("CanAttacking", false);
            yield return new WaitForSeconds(2f);
            cooling = false;
            attackMode = false;
        }
    }

    public override void Move()
    {
        base.Move(); 
        anim.SetBool("CanWalk", true);
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Feelie_attack"))
        {
            if (!flipped)
            {
                Vector2 targetPosition = new Vector2(target.position.x - 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            }
            else
            {
                Vector2 targetPosition = new Vector2(target.position.x + 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, currentSpeed * Time.deltaTime);
            }
        }
    }

    void Attack()
    {
        timer = intTimer; // reset Timer when player enters in attack range
        attackMode = true; //to check if enemy can still attack or not

        anim.SetBool("CanWalk", false);
        anim.SetBool("CanAttacking", true);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitBox.position, attackRange, enemyLayers);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (!cooling && enemy.GetComponent<PlayerHealth>() == true)
            {
                enemy.GetComponent<PlayerHealth>().DamagePlayer(20, transform, critHit);
            }
        }
    }

    void Cooldown()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.5 && cooling && attackMode)
        {
            cooling = false;
            timer = intTimer;
        }
    }

    public void TriggerCooling()
    {
        cooling = true;
    }

    private bool InsideofLimits()
    {
        return transform.position.x > leftLimit.position.x && transform.position.x < rightLimit.position.x;
    }

    public void SelectTheTarget()
    {
        float distanceToLeft = Vector3.Distance(transform.position, leftLimit.position);
        float distanceToRight = Vector3.Distance(transform.position, rightLimit.position);
        Animator lightAnim = blinkLight.GetComponent<Animator>();
        lightAnim.SetBool("ifInRange", false);
        if (distanceToLeft > distanceToRight)
        {
            target = leftLimit;
            flipped = true;
        }
        else
        {
            target = rightLimit;
            flipped = false;
        }

        coroutineStarted = false;

        StartCoroutine(Flipped());
    }

    public void Flip() //In HotZone.cs
    {
        Vector3 rotation = transform.eulerAngles;
        if (!FeelieIsDamaged)
        {
            if (transform.position.x > target.position.x)
            {
                rotation.y = 180;
                flipped = true;
            }
            else
            {
                rotation.y = 0;
                flipped = false;
            }
            transform.eulerAngles = rotation;
        }
    }

    IEnumerator Flipped()
    {
        Vector3 rotation = transform.eulerAngles;
        if (!FeelieIsDamaged)
        {
            if (transform.position.x > target.position.x)
            {
                rotation.y = 180;
                flipped = true;
            }
            else
            {
                rotation.y = 0;
                flipped = false;
            }
            transform.eulerAngles = rotation;
            yield return new WaitForSeconds(2f);
        }
    }
}
