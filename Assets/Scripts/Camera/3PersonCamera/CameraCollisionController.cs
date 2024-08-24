using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public Transform player; // El punto de referencia de tu jugador.
    public Transform cameraHolder; // El contenedor de tu cámara.
    public float cameraDistance = 5f; // La distancia deseada entre el jugador y la cámara.
    public LayerMask collisionLayers; // Las capas con las que la cámara puede colisionar.

    private Vector3 desiredCameraPosition;

    void LateUpdate()
    {
        HandleCameraCollision();
    }

    void HandleCameraCollision()
    {
        // Calcula la posición deseada de la cámara
        desiredCameraPosition = player.position - cameraHolder.forward * cameraDistance;

        // Lanza un rayo desde el jugador hacia la posición deseada de la cámara
        Ray ray = new Ray(player.position, desiredCameraPosition - player.position);
        RaycastHit hit;

        // Dibuja el rayo para análisis
        Debug.DrawRay(ray.origin, ray.direction * cameraDistance, Color.red);

        if (Physics.Raycast(ray, out hit, cameraDistance, collisionLayers))
        {
            // Si el rayo colisiona con un objeto, reposiciona la cámara cerca del punto de colisión
            cameraHolder.position = hit.point + hit.normal * 0.5f;
        }
        else
        {
            // Si no hay colisión, coloca la cámara en la posición deseada
            cameraHolder.position = desiredCameraPosition;
        }
    }

}
