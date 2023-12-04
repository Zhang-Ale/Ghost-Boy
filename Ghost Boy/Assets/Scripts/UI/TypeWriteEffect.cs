using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TypeWriteEffect : MonoBehaviour
{
    public float waitTime = 0.05f;
    public string fullText;
    private string currentText = "";
    public bool startType; 
    public bool allTyped;
    bool isCoroutineStarted;

    bool one_click = false;
    float timer_for_double_click;
    float delay = 0.5f;
    GameObject fastForwardUI; 

    void Start()
    {
        fastForwardUI = transform.GetChild(0).gameObject;
        isCoroutineStarted = false; 
    }

    IEnumerator ShowText()
    {
        isCoroutineStarted = true;
        currentText = string.Empty; 
        yield return new WaitForSeconds(0.5f);
        for (int i = 0; i <= fullText.Length; i++)
        {
            currentText = fullText.Substring(0, i);
            this.GetComponent<TextMeshProUGUI>().text = currentText;
            yield return new WaitForSeconds(waitTime);
            if (i == fullText.Length)
            {
                allTyped = true;
            }           
        }
    }

    private void Update()
    {
        if (startType & !isCoroutineStarted)
        {
            StartCoroutine(ShowText());
            fastForwardUI.SetActive(true);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (!one_click)
            {
                timer_for_double_click = Time.time; //save the current time
                one_click = true;
                //do one click things; 
            }
            else
            {
                if ((Time.time - timer_for_double_click) > delay)
                { 
                    timer_for_double_click = Time.time; 
                }
                else
                {
                    StopAllCoroutines();
                    currentText = fullText.Substring(0, fullText.Length);
                    this.GetComponent<TextMeshProUGUI>().text = currentText;
                    allTyped = true;
                    one_click = false;
                    fastForwardUI.SetActive(false);
                }
            }        
        }
    }
}
