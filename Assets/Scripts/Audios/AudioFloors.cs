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
            // Verificamos si el jugador se está moviendo
            if (playerController.IsMoving())
            {
                player.GetComponent<AudioSource>().clip = clipPasos;
                player.GetComponent<AudioSource>().Play(); // Reproduce el sonido al cambiar de superficie
            }
        }
    }
}
