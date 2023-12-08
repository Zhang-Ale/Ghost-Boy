using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Singleton<PlayerInstance>
{
    public CharacterData_SO characterData;

    #region Read from Data_SO

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
    public int CurrentLevel
    {
        get { if (characterData != null) return characterData.curLevel; else return 0; }
        set { characterData.curLevel = value; }
    }
    public int MaxLevel
    {
        get { if (characterData != null) return characterData.maxLevel; else return 0; }
        set { characterData.maxLevel = value; }
    }
    public int BaseXp
    {
        get { if (characterData != null) return characterData.baseExp; else return 0; }
        set { characterData.baseExp = value; }
    }
    public int CurrentXp
    {
        get { if (characterData != null) return characterData.curExp; else return 0; }
        set { characterData.curExp = value; }
    }
    public float LevelBuff
    {
        get { if (characterData != null) return characterData.levelBuff; else return 0; }
        set { characterData.levelBuff = value; }
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
    #region Character Combat

    public void TakeDamage()
    {

    }
    #endregion
}
