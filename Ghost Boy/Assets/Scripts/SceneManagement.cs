using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneManagement : UISubject
{
    public static SceneManagement Instance { get; set; }
    int Scene_index;
    [Tooltip("the level display")]
    public TextMeshProUGUI LevelText;
    [SerializeField] string levelName;

    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this; 
        }

        DontDestroyOnLoad(gameObject); 
    }

    public void Start()
    {
        levelName = SceneManager.GetActiveScene().name;
        if (LevelText != null)
        {
            LevelText.text = "<< " + levelName + " >>";
        }
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
        PermanentUIManager.Instance.OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex - 1;
        SceneManager.LoadSceneAsync(Scene_index);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NotifyObservers(PlayerActions.NewLevel);
        yield return null;
    }

    public IEnumerator Next_Scene()
    {
        NotifyObservers(PlayerActions.FadeIn);
        yield return new WaitForSeconds(1.25f);
        PermanentUIManager.Instance.OnLoadNewScene();
        yield return new WaitForSeconds(1.25f);
        Scene_index = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadSceneAsync(Scene_index);
        levelName = SceneManager.GetActiveScene().name;
        SetLevelName(Scene_index, levelName);
        NotifyObservers(PlayerActions.NewLevel);
        yield return null; 
    }
}
