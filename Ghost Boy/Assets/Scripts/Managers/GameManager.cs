using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public PlayerInstance playerInstance; 
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(PlayerInstance player)
    {
        playerInstance = player; 
    }

    public void RegisterCheckpoints(Checkpoint checkpoint)
    {

    }

}
