using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotifierManager : UISubject
{
    public bool notifyFadeIn;
    public bool notifyNewLevel; 
    void Update()
    {
        if (notifyFadeIn)
        {
            NotifyObservers(PlayerActions.FadeIn);
        }

        if (notifyNewLevel)
        {
            NotifyObservers(PlayerActions.NewLevel);
        }
    }
}
