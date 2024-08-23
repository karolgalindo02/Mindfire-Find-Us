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
    public PlayerControllerMF playerController;
    public CameraLook cameraLook; 

    public KeyCode skipKey = KeyCode.E;
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
        playerController.enabled = true;
        if (cameraLook != null)
            {
                cameraLook.enabled = true;
            }
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
}