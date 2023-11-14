using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using TMPro;

public class GreenBin : MonoBehaviour
{
    float progress = 0f;
    public bool _lightBlinkingOn = true;
    public Light2D _light;
    public bool _activateGuidance = false;
    public bool _stopActivate = false;
    public GameObject Instruction;

    private void Start()
    {
        _light = GameObject.Find("BinLight").GetComponent<Light2D>();        
        StartCoroutine(LightFlickering());
        _stopActivate = false;
    }

    IEnumerator LightFlickering()
    {
        while (_lightBlinkingOn == true)
        {            
            _light.intensity = 0.75f;
            yield return new WaitForSeconds(0.5f);
            _light.intensity = 0.5f;
            yield return new WaitForSeconds(0.5f);
        }                      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && _stopActivate == false)
        {
            _activateGuidance = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && _stopActivate == false)
        {
            if (Input.GetKey(KeyCode.F))
            {
                _activateGuidance = false;
                _stopActivate = true;
                _lightBlinkingOn = false;
                InvokeRepeating("Fade", 0.08f, 1);
                Instruction.SetActive(true);
                GameObject instructText = Instruction.transform.GetChild(1).gameObject;
                TextMeshProUGUI _Text = instructText.GetComponent<TextMeshProUGUI>();
                _Text.text = "Press Space to jump";
                StartCoroutine(CloseInstruction());
            }
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && _stopActivate == false)
        {
            _activateGuidance = false;
        }
    }
    IEnumerator CloseInstruction()
    {
        yield return new WaitForSeconds(3f);
        Instruction.SetActive(false);
    }
    public void Fade()
    {
        progress += 0.2f;
        _light.intensity = Mathf.Lerp(_light.intensity, 1, progress);         
    }
}
