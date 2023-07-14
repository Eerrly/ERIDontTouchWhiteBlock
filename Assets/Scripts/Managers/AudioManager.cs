using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMono<AudioManager>, IManager
{
    public bool IsInitialized { get; set; }

    private float _volume;
    public float Volume 
    {
        get
        {
            return _volume;
        }
        set
        {
            if(audioSource != null)
            {
                audioSource.volume = value;
            }
            _volume = value;
        }
    }

    private AudioClip audioClip;
    private AudioSource audioSource;

    public void OnInitialize()
    {
        audioClip = Resources.Load<AudioClip>("Audios/BackgroundMusic");
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.clip = audioClip;
        Volume = 1.0f;
        audioSource.Play();

        IsInitialized = true;
    }

    public void PauseAudio()
    {
        audioSource?.Pause();
    }

    public void UnPauseAudio()
    {
        audioSource?.UnPause();
    }

    public void PlayAudio()
    {
        audioSource?.Play();
    }

    public void StopAudio()
    {
        audioSource?.Stop();
    }

    public void OnRelease()
    {
        audioSource.Stop();
    }
}
