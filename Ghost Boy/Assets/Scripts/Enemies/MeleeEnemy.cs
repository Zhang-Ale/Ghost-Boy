using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class MeleeEnemy : MonoBehaviour
{
    [Header("Attack Parameters")]
    [SerializeField] private float attackCooldown;
    [SerializeField] private float range;
    public bool inRange;

    [Header("Collider Parameters")]
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [Header("Player Layer")]
    [SerializeField] private LayerMask playerLayer;
    private float cooldownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private PlayerHealth playerHealth;
    private EnemyPatrol enemyPatrol;

    public int maxHealth = 100;
    public int currentHealth;
    public Light2D blinkLight;
    public FeelieHpBar HealthBar;
    public bool isDamaged;
    public bool critHit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponentInParent<EnemyPatrol>();
        currentHealth = maxHealth;
        HealthBar.SetHealth(currentHealth, maxHealth);
    }

    private void Update()
    {
        HealthBar.SetHealth(currentHealth, maxHealth);
        cooldownTimer += Time.deltaTime;

        //Attack only when player in sight?
        if (PlayerInSight())
        {
            inRange = true;
            Animator lightAnim = blinkLight.GetComponent<Animator>();
            lightAnim.SetBool("ifInRange", true);
            if (cooldownTimer >= attackCooldown)
            {
                cooldownTimer = 0;
                anim.SetTrigger("CanAttacking");
            }
        }
        else
        {
            inRange = false;
        }

        if (enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInSight();

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void TakeDamage(int damage)
    {
        isDamaged = true;
        currentHealth -= damage;
        anim.SetTrigger("Damaged");
        StartCoroutine(ControlMove());
    }
    IEnumerator ControlMove()
    {
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2f);
        transform.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
        isDamaged = false;
    }

    private bool PlayerInSight()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z),
            0, Vector2.left, 0, playerLayer);

        if (hit.collider != null)
        {
            playerHealth = hit.transform.GetComponent<PlayerHealth>();
            PlayerTakeDamage();
        }
            
        return hit.collider != null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
            new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

    private void PlayerTakeDamage()
    {
        if (PlayerInSight())
            playerHealth.DamagePlayer(20, transform, critHit);
    }

    void Die()
    {
        Animator lightAnim = blinkLight.GetComponent<Animator>();
        lightAnim.SetBool("ifInRange", false);
        anim.SetBool("Dead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(this.gameObject, 2f);
    }
}
