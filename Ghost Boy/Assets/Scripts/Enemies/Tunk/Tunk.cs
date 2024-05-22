using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunk : Enemy
{
    private void Start()
    {
        damageType = DamageTypes.Tunk;

        curHealth = maxHealth;
    }

    void Update()
    {
        
    }
}
