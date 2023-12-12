using System.Collections;
using System.Collections.Generic;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement; 
using TMPro;

public class SpawnOnlyUI : MonoBehaviour
{
    [Header("GameStartUI")]
    public MenuActs menu;
    public PanelFader fullScreenPF;
    public PanelFader storyTextPF; 
    public CanvasGroup CharacterInfoUI;
    public GameObject IntroText;
    public TypeWriteEffect TWE;
    bool storyDisplaying; 
    public GameObject ContinueButton;
    public GameObject player;
    public Light2D playerLight;
    public Light2D firstLight;
    float progress = 0;
    AudioSource AS; 

    [Header("GameplayUI")] 
    public DialogueTwoImage D2Image;
    public GreenBin GB;
    public WhiteBin WB;
    public GameObject Guidance0;
    public GameObject Guidance1;
    public GameObject Guidance2;
    public GameObject Guidance3;
    public GameObject Notification1;
    public GameObject Notification2;
    public HotZoneCheck HZ;
    private bool clicked;
    public Checkpoint cp1;
    public Checkpoint cp2;
    public Guidance0Trigger G0T;
    public PlayerAttack PA;
    public GameObject fallDetector; 

    void Start()
    {
        clicked = false;
        storyDisplaying = false;
        AS = GetComponent<AudioSource>();
    }

    public void SpawnLevelStart()
    {
        if (menu.gameStart && !menu.startActivated)
        {
            storyTextPF.FadeIn();
            TWE.fullText = "Wanderer soul Benjamin. \n Welcome to the Underworld. \n Survive with your regrets. \n Your new life now begins.";
            IntroText.SetActive(true); 
            TWE.startType = true;
            storyDisplaying = true;
            menu.startActivated = true;
        }
    }

    public void SpawnGameplayStart()
    {
        if (menu.startActivated)
        {
            fallDetector.SetActive(true);
            AS.Play(); 
            storyTextPF.FadeOut();
            fullScreenPF.FadeOut();
            IntroText.SetActive(false);
            storyDisplaying = false; 
            ContinueButton.SetActive(false);
            player.SetActive(true);
            CharacterInfoUI.alpha = 1;
            InvokeRepeating("LightsOn", 0.1f, 0.2f);
            if (playerLight.intensity == 1f)
            {
                CancelInvoke();
                playerLight.intensity = 1f;
            }
        }
    }

    void LightsOn()
    {
        progress += 0.05f;
        playerLight.intensity = Mathf.Lerp(0, 0.8f, progress);
        firstLight.intensity = Mathf.Lerp(0, 1.1f, progress);
    }

    void Update()
    {
        if (storyDisplaying && TWE.allTyped)
        {
            ContinueButton.SetActive(true);
        }

        if (D2Image._activateGuidance)
        {
            Guidance2.SetActive(true);
            GameObject guideText = Guidance2.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = guideText.GetComponent<TextMeshProUGUI>();
            Text.text = "Press F to interact";
        }
        else
        {
            Guidance2.SetActive(false);
        }

        if (GB._activateGuidance)
        {
            Guidance1.SetActive(true);
            GameObject guideText = Guidance1.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = guideText.GetComponent<TextMeshProUGUI>();
            Text.text = "Press F to interact";
        }
        else
        {
            Guidance1.SetActive(false);
        }

        if (HZ.showGuide)
        {
            Guidance1.SetActive(true);
            GameObject guideText = Guidance1.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = guideText.GetComponent<TextMeshProUGUI>();
            Text.text = "Press Left Mouse Button to attack";
            if (Input.GetMouseButtonDown(0))
            {
                Guidance1.SetActive(false);
                PA = GameObject.Find("Player_prefab").GetComponent<PlayerAttack>();
                PA.canAttack = true; 
            }
        }

        if (WB.inside && !clicked)
        {
            Guidance3.SetActive(true);
            GameObject guideText = Guidance3.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = guideText.GetComponent<TextMeshProUGUI>();
            Text.text = "Press F to interact";
            if (Input.GetKey(KeyCode.F))
            {
                clicked = true;
                Guidance3.SetActive(false);
            }
        }
        else
        {
            Guidance3.SetActive(false);
        }

        if (cp1._activatedNotification)
        {
            Notification1.SetActive(true);
            GameObject notifyText = Notification1.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = notifyText.GetComponent<TextMeshProUGUI>();
            Text.text = "New checkpoint saved";
        }

        if (cp2._activatedNotification)
        {
            Notification2.SetActive(true);
            GameObject notifyText = Notification2.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = notifyText.GetComponent<TextMeshProUGUI>();
            Text.text = "New checkpoint saved";
        }

        if (G0T.activatedGuid0)
        {
            Guidance0.SetActive(true);
            GameObject guideText = Guidance0.transform.GetChild(1).gameObject;
            TextMeshProUGUI Text = guideText.GetComponent<TextMeshProUGUI>();
            Text.text = "Press AD to move horizontally";
        }
        else
        {
            Guidance0.SetActive(false);
        }
    }
}
