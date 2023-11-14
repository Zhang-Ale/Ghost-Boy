using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelFader : MonoBehaviour
{
    private bool mFaded = false;
    public float Duration = 1f;

    public void FadeIn()
    {
        CanvasGroup canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ActionOne(canvGroup, canvGroup.alpha, mFaded ? 0 : 1));
    }

    public void FadeOut()
    {
        CanvasGroup canvGroup = GetComponent<CanvasGroup>();
        StartCoroutine(ActionOne(canvGroup, canvGroup.alpha, mFaded ? 1 : 0));
    }

    public IEnumerator ActionOne(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        yield return new WaitForSeconds(0.5f);
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
        }
    }  
}
