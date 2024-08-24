using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineController : MonoBehaviour
{
    public PlayableDirector timelineDirector; // asign the playable director to active or desacive
    public GameObject timelineCameras; // asign the game object to have the cinematic cameras
    public Canvas[] canvases; // Here we can put all the canvas to manage the clean escene
    public AudioSource stepAudioSource; // Assign the AudioSource for the footstep sounds
    public AudioSource ambientMusicSource;
    public PlayerControllerMF playerController;
    public CameraLook cameraLook; 

    public KeyCode skipKey = KeyCode.E;
    public float musicDelayTime = 21f;
    public GameObject player; 
    public List<GameObject> objectsToDestroy;
    private Coroutine musicCoroutine;
        void Start()
    {
        SetCanvasesActive(false);
        timelineDirector.stopped += OnTimelineStopped;

        // Mute the footstep sounds when the cinematic starts
        stepAudioSource.mute = true;

        // Disable player control at the start of the cinematic
        playerController.enabled = false;
        if (cameraLook != null)
        {
            cameraLook.enabled = false;
        }
        StartCoroutine(StartAmbientMusicAfterDelay());
        musicCoroutine = StartCoroutine(StartAmbientMusicAfterDelay());

    }
    void Update()
    {
        // Detect if the player presses the key to skip the cinematic
        if (Input.GetKeyDown(skipKey))
        {
            SkipCinematic();
        }
    }

    void OnTimelineStopped(PlayableDirector director)
    {
        if (director == timelineDirector)
        {
            EndCinematic();
        }
    }
    void SkipCinematic()
    {
        // Stop the Timeline immediately and ensure it doesn't continue
        timelineDirector.time = timelineDirector.duration;
        timelineDirector.Evaluate();
        timelineDirector.Stop();

        if (musicCoroutine != null)
        {
            StopCoroutine(musicCoroutine); // Stop the corrutine if is ejected 
            ambientMusicSource.Play(); // Play the music ambient immediately
        }

        // Call the method that executes when the cinematic ends
        EndCinematic();
    }
     void EndCinematic()
    {
        // Deactivate the GameObject containing the Timeline cameras
        timelineCameras.SetActive(false);

        // Activate all Canvas elements after the cinematic ends or is skipped
        SetCanvasesActive(true);

        // Reactivate the footstep sounds
        stepAudioSource.mute = false;
        
        if (player != null)
        {
            player.SetActive(true);
        }

        playerController.enabled = true;
        if (cameraLook != null)
        {
            cameraLook.enabled = true;
        }

        DestroyObjectsAfterCinematic();

        // Optionally: Deactivate the PlayableDirector to stop Timeline playback
        timelineDirector.gameObject.SetActive(false);
    }
    void SetCanvasesActive(bool state)
    {

        foreach (Canvas canvas in canvases)
        {
            canvas.gameObject.SetActive(state);
        }
    }
    
     IEnumerator StartAmbientMusicAfterDelay()
    {
        // Esperar el tiempo de retraso especificado antes de iniciar la música de ambiente
        yield return new WaitForSeconds(musicDelayTime);

        // Iniciar la música de ambiente si aún no ha terminado la cinemática
        if (timelineDirector.state == PlayState.Playing)
        {
            ambientMusicSource.Play();
        }
    }
    void DestroyObjectsAfterCinematic()
    {
        // Recorrer la lista de objetos y destruirlos
        foreach (GameObject obj in objectsToDestroy)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
    }

   
}