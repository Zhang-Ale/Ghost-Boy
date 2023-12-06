using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Feelie_Behaviour : Enemy
{
    #region Public Variables
    public float attackDistance;
    public float moveSpeed;
    public float timer;
    public bool inRange; //check if player is in range
    public Transform player;
    public Transform leftLimit;
    public Transform rightLimit;
    [HideInInspector] public Transform target;
    public float playerDistance; //distance between this & player
    public GameObject hotZone;
    public GameObject triggerArea;
    public int maxHealth = 50;
    public int currentHealth;
    public AudioClip BattleMusic;
    public AudioClip BackMusic;
    public Transform attackHitBox;
    public float attackRange = 1f;
    public LayerMask enemyLayers;
    public Light2D blinkLight;
    public FeelieHpBar HealthBar;
    public bool flipped = false;
    public GameObject bloodEffect;
    #endregion

    #region Private Variables
    private Animator anim;
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
    private Music music;
    #endregion

    private void Awake()
    {
        SelectTarget();
        intTimer = timer; //to store the initial value of timer
        anim = GetComponent<Animator>();
        SR = GetComponent<SpriteRenderer>();
        originalColor = SR.color;
        currentHealth = maxHealth;
        music = GameObject.Find("Main Camera").GetComponent<Music>();
        HealthBar.SetHealth(currentHealth, maxHealth);
        flipped = false;
        damageType = DamageTypes.ghost;
    }

    void FixedUpdate()
    {
        HealthBar.SetHealth(currentHealth, maxHealth);
        if(player!= null)
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
            SelectTarget();
        }

        if (inRange && !cooling)
        {
            StartCoroutine(EnemyLogic());            
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        if (playerDistance >= 35)
        {
            currentHealth = 100;
            playerDistance = 0;
        }
    }

    IEnumerator IdleAndMove()
    {
        coroutineStarted = true;
        if (!inRange)
        {
            moveSpeed = 2f;
            yield return new WaitForSeconds(5);
            moveSpeed = 0f;
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
        currentHealth = Mathf.Max(currentHealth - damage, 0);
        //currentHealth -= damage;
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
        if(BackMusic != null)
        {
            music.ChangeBGM(BackMusic);
        }       
    }

    IEnumerator EnemyLogic()
    {
        moveSpeed = 5f;
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

    void Move()
    {
        anim.SetBool("CanWalk", true);      
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Feelie_attack"))
        {
            if (!flipped)
            {
                Vector2 targetPosition = new Vector2(target.position.x - 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
            }
            else
            {
                Vector2 targetPosition = new Vector2(target.position.x + 1f, transform.position.y);
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
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
                enemy.GetComponent<PlayerHealth>().DamagePlayer(20);
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

    public void SelectTarget()
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
