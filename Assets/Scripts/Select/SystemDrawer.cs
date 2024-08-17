using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDrawer : MonoBehaviour
{
    [SerializeField] private bool drawerOpen = false;
    [SerializeField] private float drawerOpenPosition = 0.002f;
    [SerializeField] private float drawerClosePosition = 0f;
    [SerializeField] private float smooth = 3f;
    [SerializeField] private AudioSource openDrawer;
    [SerializeField] private AudioSource closeDrawer;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void ChangeDrawerState()
    {
        drawerOpen = !drawerOpen;
        PlayDrawerSound();
    }

    void Update()
    {
        Vector3 targetPosition;
        if (drawerOpen)
        {
            targetPosition = new Vector3(initialPosition.x, initialPosition.y + drawerOpenPosition, initialPosition.z);
        }
        else
        {
            targetPosition = new Vector3(initialPosition.x, initialPosition.y + drawerClosePosition, initialPosition.z);
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smooth * Time.deltaTime);
    }

    private void PlayDrawerSound()
    {
        if (drawerOpen && openDrawer != null && !openDrawer.isPlaying)
        {
            openDrawer.Play();
        }
        else if (!drawerOpen && closeDrawer != null && !closeDrawer.isPlaying)
        {
            closeDrawer.Play();
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