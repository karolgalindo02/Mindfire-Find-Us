using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDoor : MonoBehaviour
{
    [SerializeField] private bool doorOpen = false;
    [SerializeField] private float doorOpenAngle = 100f;
    [SerializeField] private float doorCloseAngle = 0f;
    [SerializeField] private float smooth = 3f;
    [SerializeField] private AudioSource openDoor;
    [SerializeField] private AudioSource closeDoor;

    public bool IsDoorOpen => doorOpen;

    // Toggles the door state and plays the corresponding sound
    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
        PlayDoorSound();
    }

    void Update()
    {
        // Rotate the door to the open or closed position smoothly
        if (doorOpen)
        {
            Quaternion targetRotation = Quaternion.Euler(0, 0, doorOpenAngle);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation, smooth * Time.deltaTime);
        }
        else
        {
            Quaternion targetRotation2 = Quaternion.Euler(0, 0, doorCloseAngle);
            transform.localRotation = Quaternion.Slerp(transform.localRotation, targetRotation2, smooth * Time.deltaTime);
        }
    }

    // Plays the appropriate sound based on the door state
    private void PlayDoorSound()
    {
        if (doorOpen && openDoor != null && !openDoor.isPlaying)
        {
            openDoor.Play();
        }
        else if (!doorOpen && closeDoor != null && !closeDoor.isPlaying)
        {
            closeDoor.Play();
        }
    }
}