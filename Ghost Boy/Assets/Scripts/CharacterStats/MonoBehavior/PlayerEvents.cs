using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.UI;

public class PlayerEvents : MonoBehaviour
{
    public Light2D myLight;
    public bool spawnFade = true;
    public bool desertFade = false;
    public GameObject spawnblock;
    public GameObject desertblock;
    public GameObject leaveBut;
    public GameObject SpawnEndBlock;
    public GameObject Title;
    public bool leaveButtonOn;
    public ParticleSystem exitPart;
    public ParticleSystem spawnPart;
    public ParticleSystem desertPart;
    public SpriteRenderer[] desertBack;
    public SpriteRenderer[] spawnBack;
    public float fadeOutTime = 1f;
    public float fadeInTime = 1f;
    PlayerController PC;
    public bool falling = false;
    [Header("DIALOGUES")]
    public GameObject Interactable1;
    public GameObject Dialogue1;
    public GameObject Interactable2;
    public GameObject Dialogue2;
    GreenBin GB;
    DialogueTwoImage D2Image;
    public GameObject BubbleScreenEffect; 

    void Start()
    {
        myLight.intensity = 0;
        leaveButtonOn = false;
        PC = gameObject.GetComponent<PlayerController>(); 
        /*GameObject[] _spawn = GameObject.FindGameObjectsWithTag("SBF");
        for (int i = 0; i < _spawn.Length; i++)
        {
            spawnBack[i] = _spawn[i].GetComponent<SpriteRenderer>();
        }*/

        desertFade = false;
        spawnFade = true;

        GB = GameObject.Find("_green bin").GetComponent<GreenBin>();
        D2Image = Dialogue2.GetComponent<DialogueTwoImage>(); 
    }

    private void Update()
    {
        if (leaveButtonOn)
        {
            leaveBut.SetActive(true);
        }
        else
        {
            leaveBut.SetActive(false);
        }

        if (!GameManager.Instance.notSpawn)
        {
            if (GB._stopActivate)
            {
                PC.canShortJump = true; 
            }

            if (D2Image._stopActivate)
            {
                PC.canLongJump = true; 
            }
        }
        else
        {
            //add something for player when the active scene != "Spawn"
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "PlaceSwitch")
        {
            leaveButtonOn = true;   
        }
        else
        {
            leaveButtonOn = false;
        }

        if (collision.gameObject == Interactable1)
        {
            Dialogue1.SetActive(true);
            Dialogue2.SetActive(false);
        }

        if (collision.gameObject == Interactable2)
        {
            Dialogue2.SetActive(true);
            Dialogue1.SetActive(false);
        }

        if (collision.tag == "LoadPreviousLevel")
        {
            StartCoroutine(GameManager.Instance.Previous_Scene());
        }

        if (collision.tag == "LoadNextLevel")
        {

            StartCoroutine(GameManager.Instance.Next_Scene());
        }
    }
        /*if (collision.gameObject.name == "SpawnLocation")
        {
            spawnFade = true;
            if (spawnFade)
            {
                for (int _spawn = 1; _spawn <= 8; _spawn++)
                {
                    StartCoroutine(FadeIn(spawnBack[_spawn]));
                }
                spawnFade = false;
            }
        }*/

    public void LeaveSpawnPlace()
    {
        for (int _spawn = 0; _spawn <= 8; _spawn++)
        {
            StartCoroutine(FadeOut(spawnBack[_spawn]));
        }
        for (int _spawn = 0; _spawn <= 8; _spawn++)
        {
            StartCoroutine(FadeIn(desertBack[_spawn]));
        }
        leaveBut.SetActive(false);
        Instantiate(exitPart, transform.position, Quaternion.identity);
        BubbleScreenEffect.SetActive(true);
        StartCoroutine(BubbleScreen());
        exitPart.Play();
        spawnblock.SetActive(true);
        desertblock.SetActive(false);
        SpawnEndBlock.SetActive(false);
        spawnPart.Play();
        Destroy(desertPart);
        //JUMP ANIMATION
        //AFTER FALLING ON THE GROUND
        if (falling)
        {
            if (PC.isGrounded)
            {
                Title.SetActive(true);
            }
        } 
    }

    IEnumerator BubbleScreen()
    {
        yield return new WaitForSeconds(6f);
        BubbleScreenEffect.SetActive(false); 
    }

    public IEnumerator FadeOut(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        while (tmpColor.a > 0f)
        {
            tmpColor.a -= 1f * Time.deltaTime / fadeOutTime;
            _sprite.color = tmpColor;
            if (tmpColor.a <= 0f)
                tmpColor.a = 0f;
            yield return null;
        }
        _sprite.color = tmpColor;
    }

    public IEnumerator FadeIn(SpriteRenderer _sprite)
    {
        Color tmpColor = _sprite.color;
        tmpColor.a = 0;
        while (tmpColor.a < 100f)
        {
            tmpColor.a += 1f * Time.deltaTime / fadeInTime;
            _sprite.color = tmpColor;
            if (tmpColor.a >= 100f)
                tmpColor.a = 100f; 
            yield return null;
        }
        _sprite.color = tmpColor;
    }

}
