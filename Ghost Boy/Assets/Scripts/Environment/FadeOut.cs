using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public float fadeTime = 1f;
    public void fadein()
    {
        StartCoroutine(GoFadeIn(GetComponent<SpriteRenderer>()));
    }

    public void fadeout()
    {  
        StartCoroutine(GoFadeOut(GetComponent<SpriteRenderer>()));   
    }
    public IEnumerator GoFadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        while (tmpColor.a > 0f)
        {
            tmpColor.a -= 1f * Time.deltaTime / fadeTime;
            _sprite.color = tmpColor;
            if (tmpColor.a <= 0f)
                tmpColor.a = 0f;
            yield return null;
        }
        _sprite.color = tmpColor;
    }

    public IEnumerator GoFadeIn(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        tmpColor.a = 0;
        while (tmpColor.a < 100f)
        {
            tmpColor.a += 1f * Time.deltaTime / fadeTime;
            _sprite.color = tmpColor;
            if (tmpColor.a >= 100f)
                tmpColor.a = 100f;
            yield return null;
        }
        _sprite.color = tmpColor;
    }
}
