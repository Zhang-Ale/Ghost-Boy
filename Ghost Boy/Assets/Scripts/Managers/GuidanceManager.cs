﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GuidanceManager : Singleton<GuidanceManager>
{
    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    
}
