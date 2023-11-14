using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorAnim : MonoBehaviour
{
    public Animation anim;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player" && Input.GetKey(KeyCode.F))
        {
            if (this.tag == "Collectable")
            {
                this.gameObject.SetActive(false);
            }
            if(this.tag == "Interactable")
            {
                this.gameObject.SetActive(false);
                //anim.Stop();
            }
        }
    }
}