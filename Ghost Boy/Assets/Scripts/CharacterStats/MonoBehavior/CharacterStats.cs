using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Singleton<PlayerInstance>
{
    public CharacterData_SO characterData;

    #region region: Read from Data_SO

    protected override void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        GameManager.Instance.RegisterPlayer(this);
    }

    public int MaxHealth
    {
        get{ if (characterData != null) return characterData.maxHealth; else return 0; }
        //set => characterData.maxHealth = value; 
        set{ characterData.maxHealth = value; }
    }
    public int CurHealth
    {
        get { if (characterData != null) return characterData.curHealth; else return 0; }
        set { characterData.curHealth = value; }
    }
    public Characters CurCharacter
    {
        get { if (characterData != null) return characterData.curCharacter; else return 0; }
        set { characterData.curCharacter = value; }
    }
    public int Level
    {
        get { if (characterData != null) return characterData.level; else return 0; }
        set { characterData.level = value; }
    }
    public int AttackDamage
    {
        get { if (characterData != null) return characterData.attackDamage; else return 0; }
        set { characterData.attackDamage = value; }
    }
    public int MaxUltCharge
    {
        get { if (characterData != null) return characterData.maxUltCharge; else return 0; }
        set { characterData.maxUltCharge = value; }
    }
    public int CurUltCharge
    {
        get { if (characterData != null) return characterData.curUltCharge; else return 0; }
        set { characterData.curUltCharge = value; }
    }
    #endregion
}
