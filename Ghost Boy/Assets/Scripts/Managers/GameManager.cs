using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class GameManager : Singleton<GameManager>
{
    public CharacterStats playerInstance;
    public bool notSpawn; 
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void RegisterPlayer(CharacterStats player)
    {
        playerInstance = player; 
    }

    public void RegisterCheckpoints(Checkpoint checkpoint)
    {

    }
    private void Update()
    {
        if (Input.GetKey(KeyCode.Q) && Input.GetKey(KeyCode.O))
        {
            SceneManager.LoadScene("Spawn");
        }

        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Spawn")
        {
            notSpawn = true; 
        }
        else
        {
            notSpawn = false; 
        }
    }

}
