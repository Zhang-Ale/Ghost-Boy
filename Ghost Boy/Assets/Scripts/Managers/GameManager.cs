using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : Singleton<GameManager>
{
    public NotifierManager NM; 
    CharacterStats playerInstance;
    public bool notSpawn;
    int Scene_index;
    [Tooltip("the level display")]
    TextMeshProUGUI LevelText;
    string levelName;
    Scene currentScene;
    public GameObject wakeUpScreen;
    Animator wakeUpScreenAnim;
    GameObject[] foreGroundObjects;
    public string curSceneName; 

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
        currentScene = SceneManager.GetActiveScene();
        Scene_index = currentScene.buildIndex;
        levelName = currentScene.name;
        if (LevelText != null)
        {
            LevelText.text = "<< " + levelName + " >>";
        }
        wakeUpScreenAnim = wakeUpScreen.GetComponent<Animator>();
        foreGroundObjects = GameObject.FindGameObjectsWithTag("Foreground");
        for(int i = 0; i < foreGroundObjects.Length; i++)
        {
            foreGroundObjects[i].AddComponent<OcclusionTrigger>();  
        }
    }

    private void SetLevelName(int Scene_index, string LevelName)
    {
        if (LevelText != null)
        {
            LevelText.text = "<< " + LevelName + " >>";
        }
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
        curSceneName = SceneManager.GetActiveScene().name;
        Scene currentScene = SceneManager.GetActiveScene();
        if(currentScene.name != "Spawn")
        {
            notSpawn = true; 
        }
        else
        {
            notSpawn = false; 
        }

        if (isScene_CurrentlyLoaded("Spawn"))
        {
            wakeUpScreenAnim.SetTrigger("WakeUp");
            currentScene = SceneManager.GetActiveScene();
            Scene_index = currentScene.buildIndex;
        }

        if (Input.GetKeyDown(KeyCode.Q) && Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Previous_Scene());
        }

        if (Input.GetKeyDown(KeyCode.P) && Input.GetKeyDown(KeyCode.O))
        {
            StartCoroutine(Next_Scene());
        }
    }

    public IEnumerator Next_Scene()
    {
        NM.notifyFadeIn = true;
        yield return new WaitForSeconds(0.5f);
        OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(Scene_index);
        yield return new WaitForSeconds(2f);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NM.notifyNewLevel = true;
        yield return null;
    }

    public IEnumerator Previous_Scene()
    {
        NM.notifyFadeIn = true;
        yield return new WaitForSeconds(0.5f);
        OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadSceneAsync(Scene_index); 
        yield return new WaitForSeconds(2f);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NM.notifyNewLevel = true; 
        yield return null;
    }

    bool isScene_CurrentlyLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; ++i)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene != currentScene)
            {
                //the scene is already loaded
                return true;
            }
        }
        return false;//scene not currently loaded in the hierarchy
    }

    public void OnLoadNewScene()
    {
        wakeUpScreenAnim.SetTrigger("LoadingNewScene");
    }

    public void TransitionToDestination(TransitionPosition tp)
    {
        switch (tp.transitionType)
        {
            case TransitionPosition.TransitionType.SameScene:
                StartCoroutine(Transition(SceneManager.GetActiveScene().name, tp.destinationTag));
                break;
            case TransitionPosition.TransitionType.DifferentScene:
                StartCoroutine(Transition(tp.sceneName, tp.destinationTag));
                break;
        }
    }

    IEnumerator Transition(string sceneName, TransitionDestination.DestinationTag destinationTag)
    {
        NM.notifyFadeIn = true;
        if (SceneManager.GetActiveScene().name != sceneName)
        {
            OnLoadNewScene();
            yield return new WaitForSeconds(1.25f);
            SceneManager.LoadSceneAsync(sceneName);
            //yield return SceneManager.LoadSceneAsync(sceneName);
            yield return new WaitForSeconds(2f);
            levelName = SceneManager.GetActiveScene().name;
            SetLevelName(Scene_index, levelName);
            NM.notifyNewLevel = true;
            yield return null;
        }
        else
        {
            playerInstance.GetComponent<PlayerController>().enabled = false;
            yield return new WaitForSeconds(0.25f);
            playerInstance.transform.SetPositionAndRotation(GetDestination(destinationTag).transform.position, GetDestination(destinationTag).transform.rotation);
            playerInstance.GetComponent<PlayerController>().enabled = true;
            yield return null;
        }
    }

    private TransitionDestination GetDestination(TransitionDestination.DestinationTag destinationTag)
    {
        var entrances = FindObjectsOfType<TransitionDestination>();

        for (int i = 0; i < entrances.Length; i++)
        {
            if (entrances[i].destinationTag == destinationTag)
                return entrances[i];
        }
        return null;
    }
}
