using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public GameObject levelText;
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
                CanvasGroup bpCanvGroup = fullScreenPanel.GetComponent<CanvasGroup>();
                StartCoroutine(ActionOne(bpCanvGroup));
                return;

            case (PlayerActions.NewLevel):
                levelText.SetActive(true);
                CanvasGroup ltCanvGroup = levelText.GetComponent<CanvasGroup>();
                StartCoroutine(ActionOne(ltCanvGroup));
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
    public IEnumerator ActionOne(CanvasGroup canvGroup)
    {
        float counter = 0f;
        while (counter < Duration)
        {
            counter += Time.deltaTime;
            canvGroup.alpha = Mathf.Lerp(0, 1, counter / Duration);
            yield return null;
        }
        yield return new WaitForSeconds(0.25f);
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
