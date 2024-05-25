using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack")]
    public int damage;
    public float attackRange, attackRate;
    public GameObject player;
    public bool critHit; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            collision.GetComponent<PlayerHealth>()?.DamagePlayer(damage, this.transform, critHit);
        }
    }
}
