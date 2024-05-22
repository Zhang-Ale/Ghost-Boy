using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaCheck : MonoBehaviour
{
    private Feelie_Behaviour FeelieParent;
    private void Awake()
    {
        FeelieParent = GetComponentInParent<Feelie_Behaviour>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            FeelieParent.target = collision.transform;
            FeelieParent.inRange = true;
            FeelieParent.hotZone.SetActive(true);
        }
    }
}
