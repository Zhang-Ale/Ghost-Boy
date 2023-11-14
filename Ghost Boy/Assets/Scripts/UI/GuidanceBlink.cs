using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuidanceBlink : MonoBehaviour
{
    private void OnEnable()
    {
        StartBlinking();
    }

    IEnumerator GuidanceBlinking(CanvasGroup CG)
    {
        while (true)
        {
            CG.alpha = 1f;
            yield return new WaitForSeconds(0.5f);
            CG.alpha = 0.5f;
            yield return new WaitForSeconds(0.5f);
        }
    }

    void StartBlinking()
    {
        StopAllCoroutines();
        CanvasGroup GuideCG = this.GetComponent<CanvasGroup>();
        StartCoroutine(GuidanceBlinking(GuideCG));
    }
}
