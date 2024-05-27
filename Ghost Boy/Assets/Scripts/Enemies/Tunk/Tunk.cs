using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class Tunk : Enemy
{
    PhysicsCheck PC;
    [Header("Invulnerability")]
    public bool invulnerable;
    public float invulnerableDuration;
    private float invulnerableCounter;
    [Header("Stats")]
    public bool wait;
    public float waitTime;
    float waitTimeCounter;
    public bool hurt; 
    public float hurtDuration;
    float hurtCounter;
    public bool dead; 
    public Animator shieldAnim;
    public ParticleSystem ps;
    public UnityEvent<Transform> OnGetHit;

    private void Start()
    {
        PC = GetComponent<PhysicsCheck>();
        damageType = DamageTypes.Tunk;
        waitTimeCounter = waitTime;
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);

        if (PC.touchLeftWall && faceDir.x < 0 || PC.touchRightWall && faceDir.x > 0)
        {
            wait = true;
        }

        SpotPlayer(); 
        TimeCounter();

        if (spotPlayer)
        {
            currentSpeed = chaseSpeed; 
        }

        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }

        if (hurt)
        {
            currentSpeed = 0;
            hurtCounter -= Time.deltaTime;
            if(hurtCounter <= 0)
            {
                currentSpeed = chaseSpeed;
                hurt = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (!dead)
        {
            Move();
        }
        else
        {
            currentSpeed = 0; 
        }
    }

    public override void Move()
    {
        base.Move();
        anim.SetBool("walk", true);
    }

    public override void OnTakeDamage(int damage, Transform attackTrans)
    {
        base.OnTakeDamage(damage, attackTrans);

        if (invulnerable)
            return;

        if(curHealth - damage > 0)
        {
            curHealth -= damage;
            anim.SetTrigger("hurt");
            currentSpeed = 0;
            shieldAnim.SetTrigger("On");
            TriggerInvulnerable();
            OnGetHit?.Invoke(attackTrans.transform);
            if (curHealth <= maxHealth / 2)
            {
                HurtCount();
            }
            else
                return; 
        }
        else
        {
            curHealth = 0;
            OnGetHit?.Invoke(attackTrans.transform);
            dead = true; 
            anim.SetBool("dead", true);
            Instantiate(ps, transform.position, Quaternion.identity); 
        }
    }

    private void HurtCount()
    {
        if (!hurt)
        {
            hurt = true;
            hurtCounter = hurtDuration; 
        }
    }

    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    public void TimeCounter()
    {
        if (wait)
        {
            anim.SetBool("walk", false);
            waitTimeCounter -= Time.deltaTime;
            if (waitTimeCounter <= 0)
            {
                wait = false;
                anim.SetBool("walk", true);
                waitTimeCounter = waitTime;
                transform.localScale = new Vector3(faceDir.x, 1, 1);
            }
        }
    }
}
