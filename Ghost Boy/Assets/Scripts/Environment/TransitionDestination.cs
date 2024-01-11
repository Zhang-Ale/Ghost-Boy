using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionDestination : MonoBehaviour
{
    public enum DestinationTag
    {
        TRIALSCENE,
        STARTSPAWN,
        ENDSPAWN, 
        STARTHOSPITAL, 
        GAMEOVERSCENE
    }
    public DestinationTag destinationTag;
}
