using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOneImage : MonoBehaviour
{
    public GameObject _itemImage;
    public GameObject _memoryImage;

    public bool Dialogue1 = false;
    public bool showDialogue1Item = false;
    public bool showDialogue1Memory = false;

    public Animator itemAnim;
    public Animator memoryAnim;

    public Sprite CarToy;
    public Sprite CarMemory;

    void Update()
    {
       PlayAnim(); 
    }

    void PlayAnim()
    {
        if (showDialogue1Item)
        {
            _itemImage.GetComponent<Image>().sprite = CarToy;
            _itemImage.SetActive(true);
        }
        else
        {
            itemAnim.SetBool("ItemDisappear", true);
        }
            
        if (showDialogue1Memory)
        {
            _memoryImage.GetComponent<Image>().sprite = CarMemory;
            _memoryImage.SetActive(true);
        }
        else
        {
            memoryAnim.SetBool("MemoryDisappear", true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Dialogue1 = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            Dialogue1 = false;
        }
    }
}
