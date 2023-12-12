using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private CharacterStats characterStats; 
    public PlayerController PC;
    public int blinks;
    public float flashTime;
    private SpriteRenderer sr;
    public Shake shake;
    public ScreenFlash sf;
    [SerializeField]
    GameObject regenerationHP;
    public Transform regenerationPos;
    Slider healthBar;

    private void Awake()
    {
        characterStats = transform.GetComponentInParent<CharacterStats>();
    }

    void Start()
    {
        characterStats.MaxHealth = 1000;
        characterStats.CurHealth = 1000;
        if (healthBar == null)
        {
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
            healthBar.maxValue = characterStats.MaxHealth;
            healthBar.value = characterStats.CurHealth;
        }     
        sr =transform.GetChild(7). GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        healthBar.maxValue = characterStats.MaxHealth;
        healthBar.value = characterStats.CurHealth;
        Death();
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }

    public void DamagePlayer(int damage)
    {
        sf.FlashScreen();

        characterStats.CurHealth -= damage;
        SetHealth(characterStats.CurHealth);

        BlinkPlayer(blinks, flashTime);
        shake.StartCoroutine("DamagedShaking");
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

    public void Death()
    {        
        if (characterStats.CurHealth == 0 || characterStats.CurHealth <= 0)
        {
            if (0 == 0)
            {
                Respawn();
            }
        }
    }
    public void Respawn()
    {
        transform.position=PC.respawnPoint;
        characterStats.CurHealth = characterStats.MaxHealth;
        SetHealth(100); 
        RegenerationEffect();
    }

    public void RegenerationEffect()
    {
        Instantiate(regenerationHP, regenerationPos.transform.position, regenerationHP.transform.rotation);
    }
}
