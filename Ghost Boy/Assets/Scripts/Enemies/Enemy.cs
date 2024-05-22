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
    PhysicsCheck PC; 
    public Vector3 faceDir;
    protected Rigidbody2D rb;
    protected Animator anim;
    public float maxHealth;
    public float curHealth; 
    public DamageTypes damageType ;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        PC = GetComponent<PhysicsCheck>();
        currentSpeed = normalSpeed;
        curHealth = maxHealth; 
    }

    private void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        if(PC.touchLeftWall || PC.touchRightWall)
        {
            transform.localScale = new Vector3(faceDir.x, 1, 1); 
        }
    }

    private void FixedUpdate()
    {
        Move(); 
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y); 
    }

    public virtual void TakeDamage(int damage){ }
}