using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PermanentUIManager : MonoBehaviour
{
    public static PermanentUIManager Instance { get; private set; }
    [SerializeField] int sceneIndex;  
    Scene currentScene;
    Animator wakeUpScreenAnim; 
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this; 
        }
        //DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        GameObject wakeUpScreen = transform.Find("WakeUpScreen").gameObject;
        wakeUpScreenAnim = wakeUpScreen.GetComponent<Animator>();

        currentScene = SceneManager.GetActiveScene();
        sceneIndex = currentScene.buildIndex; 
    }

    /*bool ifScene_CurrentlyLoaded_inEditor(string sceneName_no_extention)
    {
        for (int i = 0; i < UnityEditor.SceneManagement.EditorSceneManager.sceneCount; ++i)
        {
            var scene = UnityEditor.SceneManagement.EditorSceneManager.GetSceneAt(i);

            if (scene.name == sceneName_no_extention)
            {
                return true;//the scene is already loaded
            }
        }
        //scene not currently loaded in the hierarchy:
        return false;
    }*/

    bool isScene_CurrentlyLoaded(/*string sceneName_no_extention*/)
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
        if (isScene_CurrentlyLoaded())
        {
            wakeUpScreenAnim.SetTrigger("WakeUp");
            currentScene = SceneManager.GetActiveScene();
            sceneIndex = currentScene.buildIndex;
        }

        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.O))
        {
            StartCoroutine(SceneManagement.Instance.Next_Scene());
        }
    }

    public void OnLoadNewScene()
    {
        wakeUpScreenAnim.SetTrigger("LoadingNewScene");
    }
}
