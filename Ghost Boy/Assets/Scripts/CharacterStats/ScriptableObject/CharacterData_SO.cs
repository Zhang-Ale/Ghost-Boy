using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Data", menuName = "Character Stats/Data")]
public class CharacterData_SO : ScriptableObject
{
    [Header("Stats Info")]
    public int curCharacter; //if 0 = Benjamin, if 1 = Charlie 
    //will have to make 2 health systems later
    public int maxHealth;
    public int curHealth;
    public int level;
    public int attackDamage; 
    public int ultimateCharge; 
}
