using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : Singleton <PlayerInstance>
{
    CharacterStats characterStats;
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        characterStats = GetComponent<CharacterStats>();
    }
}
