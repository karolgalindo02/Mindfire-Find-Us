using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource, sfxSource;
    private bool isMuted = false;

    private void Start()
    {
        // Inicialización si es necesario
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }
    }

    public void ToggleMute()
    {
        isMuted = !isMuted;
        musicSource.mute = isMuted;
        sfxSource.mute = isMuted;
    }
}