using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentUIManager : MonoBehaviour
{
    public static PermanentUIManager Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this; 
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
