using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events; 

public class MusicManager : Singleton<MusicManager>
{
    public enum MusicCondition
    {
        Travel,
        Fight,
        Drama,
        Death,
        Story,
        Victory
    }

    [System.Serializable]
    public class MusicTrack
    {
        public MusicCondition condition;
        public AudioClip musicClip;
    }

    public MusicTrack[] musicTracks;
    public float fadeDuration = 1.0f;

    private AudioSource currentSource;
    private AudioSource nextSource;
    private Dictionary<MusicCondition, AudioClip> musicDictionary;

    public MenuActs MA;

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
        currentSource = gameObject.AddComponent<AudioSource>();
        nextSource = gameObject.AddComponent<AudioSource>();

        musicDictionary = new Dictionary<MusicCondition, AudioClip>();
        foreach (var track in musicTracks)
        {
            musicDictionary[track.condition] = track.musicClip;
        }
    }

    private void Update()
    {
        if (MA.gameIsPaused)
        {
            currentSource.volume = 0.6f;
        }
    }

    public void ChangeMusic(MusicCondition condition)
    {
        if (musicDictionary.ContainsKey(condition))
        {
            StartCoroutine(FadeMusic(musicDictionary[condition]));
        }
        else
        {
            Debug.LogWarning("No music track found for condition: " + condition);
        }
    }

    private IEnumerator FadeMusic(AudioClip newClip)
    {
        // Fade out the current music
        if (currentSource.isPlaying)
        {
            float startVolume = currentSource.volume;
            for (float t = 0; t < fadeDuration; t += Time.deltaTime)
            {
                currentSource.volume = Mathf.Lerp(startVolume, 0, t / fadeDuration);
                yield return null;
            }
            currentSource.Stop();
            currentSource.volume = startVolume;
        }

        // Set the new clip to the next source and start playing
        nextSource.clip = newClip;
        nextSource.volume = 0;
        nextSource.Play();

        // Fade in the new music
        float endVolume = 1.0f;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            nextSource.volume = Mathf.Lerp(0, endVolume, t / fadeDuration);
            yield return null;
        }
        nextSource.volume = endVolume;

        // Swap the sources
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;
    }
}
