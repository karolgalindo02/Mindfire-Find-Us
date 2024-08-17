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
    
    public void ChangeDoorState()
    {
        doorOpen = !doorOpen;
        PlayDoorSound();
    }

    void Update()
    {
        if(doorOpen)
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

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "TriggerDoor")
        {
            //No Sound
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "TriggerDoor")
        {
            //No Sound
        }
    }
}