using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Nombre de la música del menú principal y la música del juego
    // public string gameMusic = "GameMusic";
    public string sFxStart = "StartButton";

    private void Start()
    {
        // Reproducir la música del menú principal al inicio
        // AudioManager.Instance.PlayMusic(mainMenuMusic);
    }

    public void PlayGame()
    {
        // // Detener la música del menú principal
        // AudioManager.Instance.StopMusic();

        // // Reproducir el efecto de sonido y la música del juego
        // AudioManager.Instance.PlaySFX(sFxStart);
        // // AudioManager.Instance.PlayMusic(gameMusic);

        // Cambiar la escena a Story
        SceneManager.LoadSceneAsync("BrayanV3.0");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
