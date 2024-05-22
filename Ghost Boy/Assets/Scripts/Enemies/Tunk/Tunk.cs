using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tunk : Enemy
{
    private void Start()
    {
        damageType = DamageTypes.Tunk;
    }

    public override void Move()
    {
        base.Move();
        anim.SetBool("walk", true);
    }

}
