using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FeelieHpBar : MonoBehaviour
{
    public Slider Slider;
    public Color Low;
    public Color High;
    public Vector3 flippedOffset;
    public Vector3 normalOffset;
    private Feelie_Behaviour FeelieParent;

    private void Start()
    {
        FeelieParent = GetComponentInParent<Feelie_Behaviour>();
    }

    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;

        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }
   
    void Update()
    {
        if (FeelieParent.inRange)
        {
            if (FeelieParent.flipped)
            {
                Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + flippedOffset);
            }
            else
            {
                Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + normalOffset);
            }
        }
    }
}
