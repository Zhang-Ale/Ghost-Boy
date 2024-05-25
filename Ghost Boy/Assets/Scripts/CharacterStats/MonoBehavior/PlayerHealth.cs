using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    private CharacterStats characterStats; 
    public PlayerController PC;
    public int blinks;
    public float flashTime;
    private SpriteRenderer sr;
    public Shake shake;
    [SerializeField]
    GameObject regenerationHP;
    public Transform regenerationPos;
    Slider healthBar;
    [Header("Invulnerability")]
    public bool invulnerable;
    public float invulnerableDuration;
    private float invulnerableCounter;
    public UnityEvent<Transform> OnGetHit;
    public UnityEvent<Transform> OnGetCritHit;
    public UnityEvent OnDeath; 

    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
    }

    void Start()
    {
        characterStats.MaxHealth = 100;
        characterStats.CurHealth = 100;
        if (healthBar == null)
        {
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
            healthBar.maxValue = characterStats.MaxHealth;
            healthBar.value = characterStats.CurHealth;
        }     
        sr =transform.GetChild(6).GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        healthBar.maxValue = characterStats.MaxHealth;
        healthBar.value = characterStats.CurHealth;

        if (invulnerable)
        {
            invulnerableCounter -= Time.deltaTime;
            if (invulnerableCounter <= 0)
            {
                invulnerable = false;
            }
        }

        regenerationHP.transform.position = transform.position;
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }

    public void DamagePlayer(int damage, Transform attacker, bool critHit)
    {
        if (invulnerable)
            return;

        if (critHit)
        {
            OnGetCritHit?.Invoke(attacker.transform);
        }

        if(characterStats.CurHealth - damage > 0)
        {
            characterStats.CurHealth -= damage;
            PC.anim.SetTrigger("Hurt");
            shake.StartCoroutine("DamagedShaking");
            BlinkPlayer(blinks, flashTime);
            TriggerInvulnerable();
            OnGetHit?.Invoke(attacker.transform);
            SetHealth(characterStats.CurHealth);
        }
        else
        {
            characterStats.CurHealth = 0;
            shake.StartCoroutine("DamagedShaking");
            BlinkPlayer(blinks, flashTime);
            SetHealth(characterStats.CurHealth);
            if(characterStats.CurHealth <= 0)
            {
                OnDeath?.Invoke();
            }
        }
    }

    public void Death()
    {
        PC.isDead = true;
        Debug.Log("dead");
        StartCoroutine(RespawnCount()); 
    }

    IEnumerator RespawnCount()
    {
        yield return new WaitForSeconds(3f);
        PC.isDead = false;
    }

    private void TriggerInvulnerable()
    {
        if (!invulnerable)
        {
            invulnerable = true;
            invulnerableCounter = invulnerableDuration;
        }
    }

    void BlinkPlayer(int numBlinks, float seconds)
    {
        StartCoroutine(DoBlinks(numBlinks, seconds));
    }

    IEnumerator DoBlinks(int numBlinks, float seconds)
    {
        yield return new WaitForSeconds(0.2f);
        for (int i = 0; i < numBlinks * 2; i++)
        {
            sr.enabled = !sr.enabled;
            yield return new WaitForSeconds(seconds);
            sr.enabled = true;
        }
    }

    public void Respawn()
    {
        transform.position = PC.respawnPoint;
        characterStats.CurHealth = characterStats.MaxHealth;
        SetHealth(100); 
        RegenerationEffect();
    }

    public void RegenerationEffect()
    {
        regenerationHP.SetActive(true);
    }
}
