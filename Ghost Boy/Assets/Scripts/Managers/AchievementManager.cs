using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : Singleton<AchievementManager>, IObserver
{
    [SerializeField] UISubject _playerSubject;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }

    public void OnNotify(PlayerActions action)
    {
        if(action == PlayerActions.Collect)
        {

        }
    }

    private void OnEnable()
    {
        //add itself to the subject's list of observers
        _playerSubject.AddObserver(this);
    }

    private void OnDisable()
    {
        //remove itself to the subject's list of observers
        _playerSubject.RemoveObserver(this);
    }

}
