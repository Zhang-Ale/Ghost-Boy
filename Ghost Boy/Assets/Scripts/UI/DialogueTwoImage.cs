using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTwoImage : MonoBehaviour
{
    public GameObject _itemImage;
    public GameObject _memoryImage;

    public bool Dialogue2 = false;
    public bool showDialogue2Item = false;
    public bool showDialogue2Memory = false;
    public bool finished = false;

    public Animator itemAnim;
    public Animator memoryAnim;

    public Sprite ChocolateBox;
    public Sprite ChocolateMemory;

    public bool _activateGuidance = false;
    public bool _stopActivate = false;
    public GameObject Instruction;

    void Update()
    {
        if (showDialogue2Item)
        {
            itemAnim.SetBool("ItemDisappear", false);
            _itemImage.GetComponent<Image>().sprite = ChocolateBox;
            itemAnim.SetBool("ItemReset", true);
            _itemImage.SetActive(true);
        }
        else
        {
            itemAnim.SetBool("ItemDisappear", true);
            itemAnim.SetBool("ItemReset", false);
            if (!ItemAnimIsPlaying())
            {
                _itemImage.SetActive(false);
            }
        }

        if (showDialogue2Memory)
        {
            memoryAnim.SetBool("MemoryDisappear", false);
            _memoryImage.GetComponent<Image>().sprite = ChocolateMemory;
            memoryAnim.SetBool("MemoryReset", true);
            _memoryImage.SetActive(true);
        }
        else
        {
            memoryAnim.SetBool("MemoryDisappear", true);
            memoryAnim.SetBool("MemoryReset", false);
            if (!MemoryAnimIsPlaying())
            {
                _memoryImage.SetActive(false);
            }
        }
        
        if(_activateGuidance)
        {
            if (Input.GetKey(KeyCode.F))
            {
                _activateGuidance = false;
                _stopActivate = true;
                StartCoroutine(Activate());
            }
        }

        if (finished)
        {
            Dialogue2 = false; 
        }
    }

    bool ItemAnimIsPlaying()
    {
        return itemAnim.GetCurrentAnimatorStateInfo(0).length >
               itemAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    bool MemoryAnimIsPlaying()
    {
        return memoryAnim.GetCurrentAnimatorStateInfo(0).length >
               memoryAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    IEnumerator Activate()
    {
        Instruction.SetActive(true);
        GameObject instructText = Instruction.transform.GetChild(1).gameObject;
        TextMeshProUGUI _Text = instructText.GetComponent<TextMeshProUGUI>();
        _Text.text = "Hold and release Space to do long jump";
        yield return new WaitForSeconds(3f);
        Instruction.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Dialogue2 = true;
        } 
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && finished && _stopActivate == false)
        {
            _activateGuidance = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" && _stopActivate == false)
        {
            _activateGuidance = false;
            Dialogue2 = false; 
        }
    }
}
