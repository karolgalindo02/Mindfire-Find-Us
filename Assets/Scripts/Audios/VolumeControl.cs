using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class VolumeControl : MonoBehaviour
{
    public Slider volumeSlider; // Slider for main audio volume
    public Slider sfxSlider; // Slider for SFX volume
    public AudioSource mainAudioSource; // Main AudioSource for background music
      public List<AudioSource> sfxAudioSources; // List of AudioSources for SFX
    public Canvas volumeCanvas; // Canvas to show/hide

    private void Start()
    {
        if (volumeSlider == null || sfxSlider == null || mainAudioSource == null || sfxAudioSources == null || volumeCanvas == null)
        {
            Debug.LogError("One or more references are not assigned in the VolumeControl script.");
            return;
        }

        // Set initial volume levels
        volumeSlider.value = mainAudioSource.volume;

        // Asignar el volumen inicial basado en el primer AudioSource en la lista (puedes cambiar esto según tus necesidades)
        if (sfxAudioSources.Count > 0)
        {
            sfxSlider.value = sfxAudioSources[0].volume;
        }

        // Add listeners to sliders
        volumeSlider.onValueChanged.AddListener(OnVolumeSliderChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);

        // Start with canvas hidden
        volumeCanvas.enabled = false;
    }

    public void ToggleVolumeCanvas()
    {
        volumeCanvas.enabled = !volumeCanvas.enabled;
    }

    private void OnVolumeSliderChanged(float value)
    {
        if (mainAudioSource != null)
        {
            mainAudioSource.volume = value;
            Debug.Log("Main Audio Volume Set To: " + value);
        }
    }

    private void OnSFXSliderChanged(float value)
    {
        foreach (var sfxAudioSource in sfxAudioSources)
        {
            if (sfxAudioSource != null)
            {
                sfxAudioSource.volume = value;
            }
        }
        Debug.Log("SFX Volume Set To: " + value);
    }

    private void AdjustMainVolume(float adjustment)
    {
        if (mainAudioSource != null)
        {
            mainAudioSource.volume = Mathf.Clamp(mainAudioSource.volume + adjustment, 0f, 1f);
            volumeSlider.value = mainAudioSource.volume; // Update slider value
        }
    }

    private void AdjustSFXVolume(float adjustment)
    {
        foreach (var sfxAudioSource in sfxAudioSources)
        {
            if (sfxAudioSource != null)
            {
                sfxAudioSource.volume = Mathf.Clamp(sfxAudioSource.volume + adjustment, 0f, 1f);
            }
        }
        // Assuming that all SFX AudioSources should have the same volume, update the slider to reflect the first one
        if (sfxAudioSources.Count > 0)
        {
            sfxSlider.value = sfxAudioSources[0].volume;
        }
    }
}