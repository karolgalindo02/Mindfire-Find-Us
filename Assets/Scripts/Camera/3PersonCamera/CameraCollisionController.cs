using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public Transform player; // El punto de referencia de tu jugador.
    public Transform cameraHolder; // El contenedor de tu c�mara.
    public float cameraDistance = 5f; // La distancia deseada entre el jugador y la c�mara.
    public LayerMask collisionLayers; // Las capas con las que la c�mara puede colisionar.

    private Vector3 desiredCameraPosition;

    void LateUpdate()
    {
        HandleCameraCollision();
    }

    void HandleCameraCollision()
    {
        // Calcula la posici�n deseada de la c�mara
        desiredCameraPosition = player.position - cameraHolder.forward * cameraDistance;

        // Lanza un rayo desde el jugador hacia la posici�n deseada de la c�mara
        Ray ray = new Ray(player.position, desiredCameraPosition - player.position);
        RaycastHit hit;

        // Dibuja el rayo para an�lisis
        Debug.DrawRay(ray.origin, ray.direction * cameraDistance, Color.red);

        if (Physics.Raycast(ray, out hit, cameraDistance, collisionLayers))
        {
            // Si el rayo colisiona con un objeto, reposiciona la c�mara cerca del punto de colisi�n
            cameraHolder.position = hit.point + hit.normal * 0.5f;
        }
        else
        {
            // Si no hay colisi�n, coloca la c�mara en la posici�n deseada
            cameraHolder.position = desiredCameraPosition;
        }
    }

}
