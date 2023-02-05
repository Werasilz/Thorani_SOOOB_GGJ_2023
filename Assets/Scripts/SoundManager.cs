using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource musicSource;
    public AudioSource effectSource;

    public AudioClip[] musicClips;
    public AudioClip[] effectClips;


    public static SoundManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayMusic(AudioClip _clip = null)
    {
        if (_clip != null)
        {
            musicSource.clip = _clip;
        }

        musicSource.Play();
    }

    public void PlayEffectOneShot(AudioClip _clip)
    {
        effectSource.PlayOneShot(_clip);
    }
}
