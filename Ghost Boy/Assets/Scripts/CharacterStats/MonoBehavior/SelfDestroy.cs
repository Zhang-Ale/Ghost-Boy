using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestroy : MonoBehaviour
{ 
    private void Update()
    {
        Destroy(this.gameObject, 2.5f);
    }
}
