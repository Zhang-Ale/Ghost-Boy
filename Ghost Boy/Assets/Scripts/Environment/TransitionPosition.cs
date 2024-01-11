using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionPosition : MonoBehaviour
{
    [Header("Transition Info")]
    public string sceneName;
    public enum TransitionType
    {
        SameScene, DifferentScene
    }   
    public TransitionType transitionType;
    public TransitionDestination.DestinationTag destinationTag;
    private bool canTrans;
    public GameObject info; 

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && canTrans)
        {
            GameManager.Instance.TransitionToDestination(this);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            canTrans = true;
            info.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            info.SetActive(false);
            canTrans = false;
        }
    }
}
