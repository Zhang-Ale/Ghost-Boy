using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInstance : Singleton <PlayerInstance>
{
    CharacterStats characterStats;
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
        characterStats = GetComponent<CharacterStats>();
    }
}
