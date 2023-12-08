using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotZoneCheck : MonoBehaviour
{
    private Feelie_Behaviour FeelieParent;
    private bool inRange;
    private Animator anim;
    public GameObject text;
    public Vector3 flippedOffset;
    public Vector3 normalOffset;
    public bool showGuide = false;

    private void Awake()
    {
        FeelieParent = GetComponentInParent<Feelie_Behaviour>();
        anim = GetComponentInParent<Animator>();
        text.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (FeelieParent.inRange)
        {
            text.SetActive(true);
            if (FeelieParent.flipped)
            {
                text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + flippedOffset);
            }
            else
            {
                text.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + normalOffset);
            }
        }
        
        if (inRange && !anim.GetCurrentAnimatorStateInfo(0).IsName("CanAttack"))
        {
            FeelieParent.Flip();
            //StartCoroutine(FeelieParent.Flip());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            inRange = true;
            showGuide = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == ("Player"))
        {
            inRange = false;
            showGuide = false;
            text.SetActive(false);
            this.gameObject.SetActive(false);
            FeelieParent.triggerArea.SetActive(true);
            FeelieParent.inRange = false;
            FeelieParent.SelectTheTarget();
        }
    }
}
