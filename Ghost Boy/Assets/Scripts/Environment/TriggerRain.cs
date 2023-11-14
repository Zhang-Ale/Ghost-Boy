using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerRain : MonoBehaviour
{
    public GameObject Rain1;
    public GameObject Rain2;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Rain1.SetActive(true);
            Rain2.SetActive(true);
        }
    }
}
