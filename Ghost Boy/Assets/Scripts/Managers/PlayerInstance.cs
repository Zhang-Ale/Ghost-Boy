using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : Singleton <PlayerInstance>
{
    CharacterStats characterStats;
    public GameObject[] charactersOnly;
    [SerializeField] int charaIndex = 0;
    int maxCharacterCount = 2;
    CharacterStats CS;
    public CharacterData_SO[] characterData;
    public PlayerAttack PA; 

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        CS = transform.GetComponentInChildren<CharacterStats>();
        for (int i = 0; i < charactersOnly.Length; i++)
        {
            characterStats = charactersOnly[i].GetComponent<CharacterStats>();
        }  
    }

    private void Update()
    {
        float mouseScrollInput = Input.GetAxis("Mouse ScrollWheel");
        bool mouseScrolled = mouseScrollInput != 0; 
        if(mouseScrolled)
        {
            ChangeCharacter(); 
        }

        if(charaIndex % 2 == 0)
        {
            charactersOnly[1].SetActive(false);
            charactersOnly[0].SetActive(true);
            CS.characterData = characterData[0];
            PA.isCharlie = false;
        }
        else
        { 
            charactersOnly[0].SetActive(false);
            charactersOnly[1].SetActive(true);
            CS.characterData = characterData[1];
            PA.isCharlie = true;
        }
    }

    void ChangeCharacter()
    {
        charaIndex++;
        charaIndex %= maxCharacterCount; 
    }
}
