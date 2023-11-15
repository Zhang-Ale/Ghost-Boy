using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollow : MonoBehaviour
{
    public Transform Follow;
    private Camera MainCamera;

    void Start()
    {
        MainCamera = Camera.main;
    }

    void Update()
    {
        if(Follow != null)
        {
            var screenPos = MainCamera.WorldToScreenPoint(Follow.position);
            transform.position = screenPos;
        }
    }
}
