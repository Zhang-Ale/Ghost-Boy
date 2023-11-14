using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public ParticleSystem particle;
    public bool encountered;
    public bool _activatedNotification = false;
    public AudioSource AS1;
    public AudioSource AS2;
    public PlayerHealth PH;

    public void Start()
    {
        encountered = true;
        //GuidanceManager.instance.RegisterCheckpoints(this);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            if (encountered)
            {
                AS1.Play();
                AS2.Play();
                Instantiate(particle, transform.position, Quaternion.identity);
                _activatedNotification = true;
                PH.RegenerationEffect();
                StartCoroutine(WaitTime());
            }
            //_activatedNotification = true;
            StartCoroutine(WaitReactivate());
        }
    }
    /*private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            StartCoroutine(Wait());
        }
    }*/

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(0.5f);
        particle.Play(true);
        encountered = !encountered;
    }

    IEnumerator WaitReactivate()
    {
        yield return new WaitForSeconds(4f);
        _activatedNotification = false; 
    }
}
