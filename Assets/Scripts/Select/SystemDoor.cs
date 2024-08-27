using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDoor : MonoBehaviour
{
    [SerializeField] private bool doorOpen = false;
    [SerializeField] private float doorOpenAngle = 100f;
    [SerializeField] private float doorCloseAngle = 0f;
    [SerializeField] private float smooth = 3f;
    [SerializeField] private AudioSource openDoor;
    [SerializeField] private AudioSource closeDoor;
    [SerializeField] private string requiredKey;
    public bool doorUnlocked = false;
    public bool IsDoorOpen => doorOpen;
    public bool IsDoorUnlocked => doorUnlocked;

    // Changes the door state if the conditions are met
    public bool ChangeDoorState(bool hasKey, bool keyInHand, string keyName)
    {
        bool stateChanged = false;

        if (doorUnlocked || (hasKey && keyInHand && keyName == requiredKey))
        {
            doorOpen = !doorOpen;
            stateChanged = true;
            PlayDoorSound();
            if (hasKey && keyInHand && keyName == requiredKey)
            {
                doorUnlocked = true;
            }
        }

        return stateChanged;
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