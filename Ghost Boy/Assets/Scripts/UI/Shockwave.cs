using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class Shockwave : MonoBehaviour
{
    [SerializeField] private float _shockwaveTime = 0.75f;
    [SerializeField] private Material _material;
    Coroutine shockCor; 
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_ShockwaveFromCenter");

    void Awake()
    {
        _material = GetComponent<Image>().material;  
    }

    private void Update()
    {
        //Ask Professor
        if (Input.GetKeyDown(KeyCode.E))
        {
            CallShockwave();
        }
    }

    public void CallShockwave()
    {
        shockCor = StartCoroutine(ShockwaveAction(-0.1f, 1f));
        Debug.Log("D");
    }

    public IEnumerator ShockwaveAction(float startPos, float endPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, startPos);

        float lerpedAmount = 0f;
        float elapsedTime = 0f; 
        while(elapsedTime < _shockwaveTime)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, (elapsedTime / _shockwaveTime));
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            yield return null; 
        }
    }
}
