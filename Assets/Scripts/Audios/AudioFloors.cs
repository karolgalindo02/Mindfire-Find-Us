using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AudioFloors : MonoBehaviour
{
    public AudioClip clipPasos;
    public GameObject player; 
    private PlayerController playerController;
   
 private void Start()
    {
        // Asignamos el componente PlayerController
        playerController = player.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            player.GetComponent<AudioSource>().clip = clipPasos;

            // Si el jugador se está moviendo, reproducimos el sonido de pasos
            if (playerController.IsMoving() && !player.GetComponent<AudioSource>().isPlaying)
            {
                player.GetComponent<AudioSource>().Play(); 
            }
        }
    }
}
