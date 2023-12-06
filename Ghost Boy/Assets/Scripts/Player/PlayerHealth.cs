using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int curHealth = 0;
    public int maxHealth = 100;
    [SerializeField] Slider healthBar;

    public PlayerController PC;
    public int blinks;
    public float flashTime;
    private SpriteRenderer sr;
    public Shake shake;
    public ScreenFlash sf;
    [SerializeField]
    GameObject regenerationHP;
    public Transform regenerationPos;

    void Start()
    {
        if(healthBar == null)
        {
            healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<Slider>();
            healthBar.maxValue = maxHealth;
            healthBar.value = curHealth;
        }     
        sr = GetComponent<SpriteRenderer>();
    }

    public void Update()
    {
        Death();
    }

    public void SetHealth(int hp)
    {
        healthBar.value = hp;
    }

    public void DamagePlayer(int damage)
    {
        sf.FlashScreen();

        curHealth -= damage;
        SetHealth(curHealth);

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
        if (curHealth == 0 || curHealth <= 0)
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
        curHealth = maxHealth;
        SetHealth(100); 
        RegenerationEffect();
    }

    public void RegenerationEffect()
    {
        Instantiate(regenerationHP, regenerationPos.transform.position, regenerationHP.transform.rotation);
    }
}
