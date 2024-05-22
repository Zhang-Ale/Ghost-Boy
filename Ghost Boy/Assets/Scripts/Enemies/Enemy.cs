using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Feelie,
    Tunk,
    wall, 
    rock
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

        currentSpeed = normalSpeed;
    }

    void Update()
    {
        faceDir = new Vector3(-transform.localScale.x, 0, 0);
        Move();
    }

    public virtual void Move()
    {
        rb.velocity = new Vector2(currentSpeed * faceDir.x * Time.deltaTime, rb.velocity.y); 
    }

    public virtual void TakeDamage(int damage){ }
}