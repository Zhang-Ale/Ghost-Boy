using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerObserverSystem : MonoBehaviour, IObserver
{
    [SerializeField] UISubject _playerSubject;
    [SerializeField] int _jumpCount = 0;
    int _jumpAudioThreshold = 3;
    Coroutine _currentJumpResetRoutine = null;
    int index;
    AudioSource _audioPlayer;
    public GameObject fullScreenPanel;
    public float Duration = 1f;
    private bool mFaded = false;
    public GameObject levelText;
    CanvasGroup bpCanvGroup;
    void Start()
    {
        _audioPlayer = GetComponent<AudioSource>();
        index = SceneManager.GetActiveScene().buildIndex;
    }

    public void OnNotify(PlayerActions action)
    {
        switch (action)
        {
            case (PlayerActions.Jump):
                if (_currentJumpResetRoutine != null)
                {
                    StopCoroutine(_currentJumpResetRoutine);
                }
                _jumpCount += 1;
                if (_jumpCount == _jumpAudioThreshold)
                {
                    //something happens
                }
                _currentJumpResetRoutine = StartCoroutine(IJumpResetRoutine());
                return;

            case (PlayerActions.FadeIn):
                fullScreenPanel.SetActive(true);
                bpCanvGroup = fullScreenPanel.GetComponent<CanvasGroup>();
                StartCoroutine(ActionOne(bpCanvGroup, bpCanvGroup.alpha, mFaded ? 0 : 1));
                return;

            case (PlayerActions.NewLevel):
                //check if there's only 1 global light 2d
                CanvasGroup ltCanvGroup = levelText.GetComponent<CanvasGroup>();
                StartCoroutine(ActionOne(ltCanvGroup, ltCanvGroup.alpha, mFaded ? 0 : 1));
                bpCanvGroup = fullScreenPanel.GetComponent<CanvasGroup>();
                StartCoroutine(ActionOne(bpCanvGroup, bpCanvGroup.alpha, mFaded ? 1 : 0));
                return;

            case (PlayerActions.ContinueButton):

                return;

            case (PlayerActions.DialogueStart):
                
                return;

            case (PlayerActions.DialogueOver):
                
                return;

            default:
                return;
        }
    }
    public IEnumerator ActionOne(CanvasGroup canvGroup, float start, float end)
    {
        float counter = 0f;
        yield return new WaitForSeconds(0.5f);
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(start, end, counter / Duration);
            yield return null;
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

    IEnumerator IJumpResetRoutine()
    {
        yield return new WaitForSeconds(2.75f);
        _jumpCount = 0;
    }
}
