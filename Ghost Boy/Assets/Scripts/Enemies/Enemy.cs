using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Feelie,
    Tunk,
}

public interface IDamageable
{
    public void TakeDamage(int damage);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    [Header("Basic parameters")]
    public float normalSpeed;
    public float chaseSpeed;
    public float currentSpeed;
    protected Vector3 faceDir;
    protected Rigidbody2D rb;
    protected Animator anim;
    [Header("Attack")]
    public DamageTypes damageType;
    Transform attacker;
    [Header("Health")]
    public float maxHealth;
    public float curHealth;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        currentSpeed = normalSpeed;
        curHealth = maxHealth;
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y); 
    }

    public virtual void OnTakeDamage(int damage, Transform attackTrans)
    {
        attacker = attackTrans;
        if (attackTrans.position.x - transform.position.x > 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (attackTrans.position.x - transform.position.x < 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

    public virtual void TakeDamage(int damage) { }
}