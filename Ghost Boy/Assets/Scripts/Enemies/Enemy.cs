using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

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
    public Vector2 boxSize = new Vector2(5, 5);
    public Vector2 boxOffset = Vector2.zero;   
    [Header("Attack")]
    public DamageTypes damageType;
    public LayerMask playerLayer;
    public bool spotPlayer;
    Transform attacker;
    public UnityEvent OnSpotPlayer;
    public UnityEvent OnIdle; 
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

    public void SpotPlayer()
    {
        Vector2 boxCenter = (Vector2)transform.position + boxOffset;

        Collider2D[] hitColliders = Physics2D.OverlapBoxAll(boxCenter, boxSize, 0, playerLayer);

        bool playerSpotted = false;
        foreach (Collider2D collider in hitColliders)
        {
            if (collider.CompareTag("Player"))
            {
                playerSpotted = true;
                break;
            }
        }

        if (playerSpotted != spotPlayer)
        {
            spotPlayer = playerSpotted;
            if (spotPlayer)
            {
                MusicManager.Instance.ChangeMusic(MusicManager.MusicCondition.Fight);
            }
            else
            {
                MusicManager.Instance.ChangeMusic(MusicManager.MusicCondition.Travel);
            }
        }
    }

    public virtual void TakeDamage(int damage) { }
}