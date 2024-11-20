using System;
using System.Collections;
using System.Collections.Generic;
using SonicBloom.Koreo;
using SonicBloom.Koreo.Players;
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

    private AudioSource audioSource;
    private Koreographer koreographer;
    private SimpleMusicPlayer simpleMusicPlayer;

    public Action<KoreographyEvent> OnKoreographyEventAction;

    public void OnInitialize()
    {
        koreographer = gameObject.AddComponent<Koreographer>();
        simpleMusicPlayer = gameObject.AddComponent<SimpleMusicPlayer>();
        audioSource = gameObject.GetComponent<AudioSource>();
        
        audioSource.loop = true;
        Volume = 1.0f;
        
        koreographer.RegisterForEvents("TestKoreographyTrack", OnKoreographyEvent);
        simpleMusicPlayer.LoadSong(Resources.Load<Koreography>("Audios/BackgroundMusicKoreography"), 0, false);
        simpleMusicPlayer.Play();

        IsInitialized = true;
    }

    private void OnKoreographyEvent(KoreographyEvent koreographyEvent)
    {
        OnKoreographyEventAction?.Invoke(koreographyEvent);
    }

    public void PauseAudio()
    {
        audioSource?.Pause();
    }

    public void UnPauseAudio()
    {
        audioSource?.UnPause();
    }

    public void SetAudio(string koreographyPath, bool loop)
    {
        audioSource.loop = loop;
        var koreography = Resources.Load<Koreography>(koreographyPath);
        simpleMusicPlayer.LoadSong(koreography, 0, false);
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
