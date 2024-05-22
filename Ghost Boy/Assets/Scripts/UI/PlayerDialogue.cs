using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerDialogue : MonoBehaviour
{
    DialogueSystem dialogue;
    public GameObject dialogBox;
    public TextMeshProUGUI dialogBoxText;
    private bool _isPlayerInside;
    public PlayerController PC;
    public bool talked = false;
    public DialogueOneImage D1Image;
    public DialogueTwoImage D2Image;

    bool one_click = false;
    float timer_for_double_click;
    float delay = 0.5f;

    private void Start()
    {
        dialogue = DialogueSystem.instance;
    }

    public string[] s = new string[]
    {
        "Benjamin opens his eyes, finds himself in a deserted alley.:Benjamin",
        "Who am I...",
        "Where is this place..."
    };

    int index = 0;

    void Update()
    {
        if (_isPlayerInside)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if(!dialogue.isSpeaking || dialogue.isWaitingForUserInput)
                {
                    if (D1Image.Dialogue1 && index == 1)
                    {
                        D1Image.showDialogue1Item = true;
                    }

                    if (D2Image.Dialogue2 && index == 1)
                    {
                        D2Image.showDialogue2Item = true;
                    }

                    if (D1Image.Dialogue1 && index == 2)
                    {
                        D1Image.showDialogue1Item = false;
                        D1Image.showDialogue1Memory = true;
                    }

                    if (D2Image.Dialogue2 && index == 3)
                    {
                        D2Image.showDialogue2Item = false;
                        D2Image.showDialogue2Memory = true;
                    }

                    if (index >= s.Length)
                    {
                        if (D1Image.Dialogue1)
                        {
                            D1Image.showDialogue1Memory = false;
                        }
                            
                        if (D2Image.Dialogue2)
                        {
                            D2Image.showDialogue2Memory = false;
                            D2Image.finished = true;
                        }

                        dialogBox.SetActive(false);
                        dialogBoxText.text = "- Click to continue -";
                        PC.movementSpeed = 10f;
                        PC.flyForce = 4.5f;
                        PC.jumpForce = 12f; 
                        return; 
                    }

                    Say(s [index]);
                    index++;
                }
            }
        }
    }

    void Say(string s)
    {
        string[] parts = s.Split(':');
        string speech = parts[0];
        string speaker = (parts.Length >= 2) ? parts[1] : "";

        dialogue.Say(speech, true, speaker);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(!talked && collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            _isPlayerInside = true;
            dialogBox.SetActive(true);
            PC.movementSpeed = 0;
            PC.flyForce = 0;
            PC.jumpForce = 0; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.GetType().ToString() == "UnityEngine.BoxCollider2D")
        {
            _isPlayerInside = false;
            talked = true;
        }
    }
}
