using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SceneTitle : MonoBehaviour
{
    public TMP_Text Title;
    private bool DesertPass1;
    private bool DesertPass2;
    private bool CityPass1;
    private bool CityPass2;
    private bool BuildingPass1;
    private bool BuildingPass2;
    private bool CavePass1;
    private bool CavePass2;
    void Start()
    {
        Title.text = "";
        Title.faceColor = new Color32(255, 128, 0, 0);
    }

    void Update()
    {
        StartCoroutine(TitleChange());
    }

    IEnumerator TitleChange()
    {
        if (DesertPass1 && DesertPass2 == true)
        {
            Title.text = "Wanderer's Hollow";
            Title.faceColor = new Color32(255, 128, 0, 255);
            yield return new WaitForSeconds(3f);
            Title.faceColor = new Color32(255, 128, 0, 0);
        }

        if (CityPass1 && CityPass2 == true)
        {
            Title.text = "City Of Awakening";
            Title.faceColor = new Color32(255, 128, 0, 255);
            yield return new WaitForSeconds(3f);
            Title.faceColor = new Color32(255, 128, 0, 0);
        }

        if (BuildingPass1 && BuildingPass2 == true)
        {
            Title.text = "Unknown Castle";
            Title.faceColor = new Color32(255, 128, 0, 255);
            yield return new WaitForSeconds(3f);
            Title.faceColor = new Color32(255, 128, 0, 0);
        }

        if (CavePass1 && CavePass2 == true)
        {
            Title.text = "Underground Cave";
            Title.faceColor = new Color32(255, 128, 0, 255);
            yield return new WaitForSeconds(3f);
            Title.faceColor = new Color32(255, 128, 0, 0);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.name == "")
        {
            DesertPass1 = true;
        }
        if (other.gameObject.name == "")
        {
            DesertPass2 = true;
        }

        if (other.gameObject.name == "")
        {
            CityPass1 = true;
        }
        if (other.gameObject.name == "")
        {
            CityPass2 = true;
        }

        if (other.gameObject.name == "")
        {
            BuildingPass1 = true;
        }
        if (other.gameObject.name == "")
        {
            BuildingPass2 = true;
        }

        if (other.gameObject.name == "")
        {
            CavePass1 = true;
        }
        if (other.gameObject.name == "")
        {
            CavePass2 = true;
        }
    }
}
