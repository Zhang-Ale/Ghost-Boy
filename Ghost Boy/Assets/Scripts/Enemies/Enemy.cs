using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTypes
{
    Feelie,
    wall, 
    rock
}

public interface IDamageable
{
    public void TakeDamage(int damage);
}

public abstract class Enemy : MonoBehaviour, IDamageable
{
    public DamageTypes damageType ; 
    [SerializeField] protected float _life = 10;
    //static float _allLife = 10;//change life for all enemies (can be used for achievement of how many enemies have hit)
    public virtual void TakeDamage(int damage)
    {
        _life = Mathf.Max(_life - damage, 0);
    }
}

/*public class Ghost: Enemy
{
    private void Start()
    {
        _life = 100f;
        damageType = DamageTypes.ghost; 
    }
}*/
