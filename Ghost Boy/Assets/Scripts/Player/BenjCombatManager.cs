using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenjCombatManager : MonoBehaviour
{
    public static BenjCombatManager instance;
    public bool canReceiveInput = true;
    public bool inputReceived;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {
        canReceiveInput = true;
    }
    public void Update()
    {
        Attack();
    }
    public void Attack()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Received");
            if (canReceiveInput)
            {
                inputReceived = true;
                canReceiveInput = false;
            }
            else
            {
                return;
            }
        }
    }

    public void InputManager()
    {
        if (!canReceiveInput)
        {
            canReceiveInput = true;
        }
        else
        {
            canReceiveInput = false;
        }
    }
}
