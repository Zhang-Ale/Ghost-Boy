using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; 

public class MenuActs : MonoBehaviour
{
    [Header("StartMenu")]
    public bool gameStart = false;
    public bool startMenuOpen = true;
    public bool achieMenuOpen = false;
    public bool startActivated = false;
    public GameObject startMenu, achieMenu;
    public GameObject soulsPage, collectablesPage, charactersPage;
    public Image[] tabButtonImages; 
    AudioSource AS;

    [Header("PauseMenu")]
    public bool gameIsPaused;
    CanvasGroup fullScreenCanv;
    public GameObject ESCToMenu;
    public GameObject ResumeBut;
    string sceneName;

    public void Start()
    {
        AS = GetComponent<AudioSource>();
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
        AS.Play();
        ResumeBut.SetActive(true);
        ESCToMenu.SetActive(true);
        fullScreenCanv.alpha = 1f;
    }
    public void ResumeGame()
    {
        gameIsPaused = false;
        Time.timeScale = 1;
        AS.Play();
        ResumeBut.SetActive(false);
        ESCToMenu.SetActive(false);
        fullScreenCanv.alpha = 0f;
    }
    public void ReturnStartMenu()
    {
        ResumeBut.SetActive(false);
        ESCToMenu.SetActive(false);
        fullScreenCanv.alpha = 1f;
        AS.Play();
        startMenu.SetActive(true);
        startMenuOpen = true;
        Time.timeScale = 0;
    }
    public void PlayGame()
    {
        AS.Play();
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
        AS.Play();
        startMenu.SetActive(false);
        startMenuOpen = false;
        achieMenu.SetActive(true);
        achieMenuOpen = true;
    }
    public void Souls()
    {
        AS.Play();
        charactersPage.SetActive(false);
        collectablesPage.SetActive(false);
        tabButtonImages[0].color = Color.white;
        tabButtonImages[1].color = Color.grey;
        tabButtonImages[2].color = Color.grey;
        soulsPage.SetActive(true);
    }
    public void Collectables()
    {
        AS.Play();
        soulsPage.SetActive(false);
        charactersPage.SetActive(false);
        tabButtonImages[1].color = Color.white;
        tabButtonImages[0].color = Color.grey;
        tabButtonImages[2].color = Color.grey;
        collectablesPage.SetActive(true);
    }
    public void Characters()
    {
        AS.Play();
        collectablesPage.SetActive(false);
        soulsPage.SetActive(false);
        tabButtonImages[2].color = Color.white;
        tabButtonImages[1].color = Color.grey;
        tabButtonImages[0].color = Color.grey;
        charactersPage.SetActive(true);
    }
    public void ExitAchieMenu()
    {
        AS.Play();
        achieMenu.SetActive(false);
        achieMenuOpen = false;
        startMenu.SetActive(true);
        startMenuOpen = true;
    }
    
    public void ExitGame()
    {
        AS.Play();
        Application.Quit();
    }
}
