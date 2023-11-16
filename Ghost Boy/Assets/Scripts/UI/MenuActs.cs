using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuActs : MonoBehaviour
{
    [Header("StartMenu")]
    public bool gameStart = false;
    public bool startMenuOpen = true;
    public bool achieMenuOpen = false;
    public bool startActivated = false;
    public GameObject startMenu;
    public GameObject achieMenu;
    public GameObject soulStoriesMenu;
    public GameObject collectablesMenu; 
    [Header("PauseMenu")]
    public bool gameIsPaused;
    CanvasGroup fullScreenCanv;
    public GameObject ESCToMenu;
    public GameObject ResumeBut;
    string sceneName;

    public void Start()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        sceneName = currentScene.name;
        fullScreenCanv = GameObject.Find("FullScreenPanel").GetComponent<CanvasGroup>();
    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }
    public void PauseGame()
    {
        gameIsPaused = true;
        Time.timeScale = 0;
        ResumeBut.SetActive(true);
        ESCToMenu.SetActive(true);
        fullScreenCanv.alpha = 1f;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        ResumeBut.SetActive(false);
        ESCToMenu.SetActive(false);
        fullScreenCanv.alpha = 0f;
    }
    public void ReturnStartMenu()
    {
        ResumeBut.SetActive(false);
        ESCToMenu.SetActive(false);
        fullScreenCanv.alpha = 1f;
        startMenu.SetActive(true);
        startMenuOpen = true;
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        startMenu.SetActive(false);
        startMenuOpen = false;
        if (!startActivated && sceneName == "Spawn")
        {
            gameStart = true;
        }
        else 
        {
            Time.timeScale = 1;
            fullScreenCanv.alpha = 0f;
        }
    }
    public void AchieveMenu()
    {
        startMenu.SetActive(false);
        startMenuOpen = false;
        achieMenu.SetActive(true);
        achieMenuOpen = true;
    }
    public void Stories()
    {
        achieMenu.SetActive(false);
        achieMenuOpen = false;
        soulStoriesMenu.SetActive(true);
    }
    public void Collectables()
    {
        achieMenu.SetActive(false);
        achieMenuOpen = false;
        collectablesMenu.SetActive(true);
    }
    public void ExitAchieMenu()
    {
        achieMenu.SetActive(false);
        achieMenuOpen = false;
        startMenu.SetActive(true);
        startMenuOpen = true;
    }
    public void ExitSoulStoriesMenu()
    {
        soulStoriesMenu.SetActive(false);
        achieMenu.SetActive(true);
        achieMenuOpen = true;       
    }
    public void ExitCollectablesMenu()
    {
        collectablesMenu.SetActive(false);
        achieMenu.SetActive(true);
        achieMenuOpen = true;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
}
