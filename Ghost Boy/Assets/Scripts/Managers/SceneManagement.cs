using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : UISubject
{
    int Scene_index;
    [Tooltip("the level display")]
    TextMeshProUGUI LevelText;
    string levelName;
    Scene currentScene;
    public GameObject wakeUpScreen;
    Animator wakeUpScreenAnim;

    public void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        Scene_index = currentScene.buildIndex;
        levelName = currentScene.name;
        if (LevelText != null)
        {
            LevelText.text = "<< " + levelName + " >>";
        }
        wakeUpScreenAnim = wakeUpScreen.GetComponent<Animator>();
    }

    private void SetLevelName(int Scene_index, string LevelName)
    {
        if (LevelText != null)
        {
            LevelText.text = "<< " + LevelName + " >>";
        }
    }

    public IEnumerator Previous_Scene()
    {
        NotifyObservers(PlayerActions.FadeIn);
        yield return new WaitForSeconds(1.25f);
        OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadSceneAsync(Scene_index);
        yield return new WaitForSeconds(2f);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NotifyObservers(PlayerActions.NewLevel);
        yield return null;
    }

    public IEnumerator Next_Scene()
    {
        NotifyObservers(PlayerActions.FadeIn);
        yield return new WaitForSeconds(1.25f);
        OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(Scene_index);
        yield return new WaitForSeconds(2f);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NotifyObservers(PlayerActions.NewLevel);
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

    void Update()
    {
        if (isScene_CurrentlyLoaded("Spawn"))
        {
            wakeUpScreenAnim.SetTrigger("WakeUp");
            currentScene = SceneManager.GetActiveScene();
            Scene_index = currentScene.buildIndex;
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.O))
        {
            StartCoroutine(Next_Scene());
        }
    }

    public void OnLoadNewScene()
    {
        wakeUpScreenAnim.SetTrigger("LoadingNewScene");
    }
}
