using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [Header("Attack")]
    public int damage;
    public float attackRange, attackRate;
    public LayerMask playerLayer;
    public bool critHit; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(((1 << collision.gameObject.layer) & playerLayer) != 0)
        {
            collision.GetComponent<PlayerHealth>()?.DamagePlayer(damage, this.transform, critHit);
        }
    }
}
