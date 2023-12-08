using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public Characters curCharacter;
    //will have to make 2 health systems later
    public int maxHealth;
    public int curHealth;
    public int attackDamage;
    public int maxUltCharge;
    public int curUltCharge;
    [Header("Kill")]
    public int killPoint;
    [Header("Leveling up")]
    public int curLevel;
    public int maxLevel;
    public int baseExp;
    public int curExp;
    public float levelBuff;

    public float levelMultiplier
    {
        get { return 1 + (curLevel - 1) * levelBuff; }
    }

    public void UpdateExp(int point)
    {
        curExp += point; 
        if(curExp >= baseExp)
        {
            LevelUp(); 
        }
    }
    private void LevelUp()
    {
        curExp = Mathf.Clamp(curExp + 1, 0, maxLevel);
        baseExp += (int)(baseExp * levelMultiplier);
        maxHealth = (int)(maxHealth * levelMultiplier);
        curHealth = maxHealth; 
        //increase maxHealth
        //increase attackDamage
        //add buffs or skills
    }
}

public enum Characters
{
    Benjamin, 
    Charlie,
    Feelie
}