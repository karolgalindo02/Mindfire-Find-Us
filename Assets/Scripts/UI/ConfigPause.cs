using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class ConfigPause : MonoBehaviour
{
    public VolumeControl volumeControl; 
    public GameObject menuObjPause;
    public bool pause=false;
    public GameObject OptionsMenu; 
    public GameObject creditsMenu;
    public GameObject homeExit;
    public GameObject Canvas;
    
     private AudioSource[] sonidos;
    private AudioSource[] efectosSonidos;
    void Start()
    {
        sonidos = FindObjectsOfType<AudioSource>();
        
        efectosSonidos = sonidos.Where(s => s.CompareTag("EffectSound") || s.CompareTag("Player")).ToArray();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!pause)
            {
                Canvas.SetActive(false);
                PausarJuego();
            }
            else
            {
                Canvas.SetActive(true);
                Resume();
            }
        }
    }

    public void PausarJuego()
    {
        menuObjPause.SetActive(true);
        pause = true;

        // Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        // Pause all the sounds
        foreach (AudioSource sonido in sonidos)
        {
            sonido.Pause();
        }
        if (volumeControl != null)
        {
            volumeControl.ToggleVolumeCanvas();
        }
    }

public void Resume()
{
    OptionsMenu.SetActive(false);
    menuObjPause.SetActive(false);
    creditsMenu.SetActive(false);
    homeExit.SetActive(false);
    pause = false;
    Time.timeScale = 1;
    Cursor.visible = false;
    Cursor.lockState = CursorLockMode.Locked;

    AudioSource[] sonidos = FindObjectsOfType<AudioSource>();
                
    foreach (AudioSource sonido in sonidos)
    {
        // Play all audio sources except those tagged with "EffectSound" or "Player" during resume
        if (!sonido.CompareTag("EffectSound") && !sonido.CompareTag("Player"))
        {
            sonido.Play();
        }
    }
    if (volumeControl != null)
        {
            volumeControl.ToggleVolumeCanvas();
        }
    Canvas.SetActive(true);
}

    public void GoToMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
        ResumeGameAndLoadScene();
    }
    private void ResumeGameAndLoadScene()
    {
        // Restablece el estado del juego antes de cargar una nueva escena
        Time.timeScale = 1;
        pause = false;
    }
}