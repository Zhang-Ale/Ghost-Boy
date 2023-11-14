using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guidance0Trigger : MonoBehaviour
{
    public bool activatedGuid0 = false;
    private void Update()
    {
        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            activatedGuid0 = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            activatedGuid0 = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            activatedGuid0 = false;
        }
    }
}
