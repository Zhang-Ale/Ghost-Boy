using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : Singleton<Music>
{
    public AudioSource _music;
    public AudioClip backgroundMusic;
    public AudioClip battleMusic;
    Feelie_Behaviour Feelie;
    bool playBack = false;
    bool playBattle = false;
    public MenuActs MA;
    public bool playBackMusic;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        Feelie = GameObject.Find("Feelie_enemy").GetComponent<Feelie_Behaviour>();
        _music.clip = backgroundMusic;
        _music.Play();
        playBack = true;
    }

    private void Update()
    {
        if (MA.gameIsPaused)
        {
            _music.volume = 0.6f;
        }

        if (Feelie.inRange == true)
        {
            playBack = false;
            if (!playBattle)
            {
                playBackMusic = false; 
                StartCoroutine(PlayBattleMusic());
                playBattle = false;
            }
        }
        if (Feelie.inRange == false)
        {
            playBattle = false;
            if (!playBack)
            {
                playBackMusic = true; 
                playBack = false;
            }
        }

        if (playBackMusic)
        {
            StartCoroutine(PlayBackMusic());
        }
        else
        {
            StopCoroutine(PlayBackMusic());
        }
    }

    IEnumerator PlayBattleMusic()
    {
        yield return new WaitForSeconds(0.4f);
        if (Feelie.inRange == true)
        {
            float startVolume = _music.volume;
            _music.volume -= startVolume * Time.deltaTime / 1f;
            //_music.Stop();
            _music.clip = battleMusic;
            _music.volume = 0.5f;
            _music.Play();
            playBattle = true;
        }
    }

    IEnumerator PlayBackMusic()
    {
        yield return new WaitForSeconds(1f);
        if (Feelie.inRange == false)
        {
            float startVolume = _music.volume;
            _music.volume -= startVolume * Time.deltaTime / 1f;
            //_music.Stop();
            playBattle = false;
            _music.clip = backgroundMusic;
            _music.volume = 1f;
            _music.Play();
            playBack = true;
        }
    }

    public void ChangeBGM(AudioClip music)
    {
        _music.Stop();
        _music.clip = music;
        _music.Play();
    } 
}
