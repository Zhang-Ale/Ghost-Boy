using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WhiteBin : MonoBehaviour
{
    public GameObject video;
    public bool inside = false;
    public GameObject Description;
    public GameObject collectable1; 

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            inside = true;
            
            if (Input.GetKey(KeyCode.F))
            {
                StartCoroutine(VideoPlay());
                collectable1.SetActive(true); 
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inside = false;
        }
    }

    IEnumerator VideoPlay()
    {
        yield return new WaitForSeconds(0.1f);
        video.SetActive(true);
        yield return new WaitForSeconds(13f);
        video.SetActive(false);
        yield return new WaitForSeconds(1f);
        Description.SetActive(true);
        yield return new WaitForSeconds(4f);
        Description.SetActive(false);
    }
}
