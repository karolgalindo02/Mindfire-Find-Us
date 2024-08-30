using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public Transform player; // Reference point of the player.
    public Transform cameraHolder; // Container of the camera.
    public float cameraDistance = 2f; // Distance between the player and the camera.
    public LayerMask collisionLayers; // The layers that the camera can collide with..
    public float smoothTime = 0.1f; // Tiempo de suavizado

    private Vector3 velocity = Vector3.zero;
    private Vector3 desiredCameraPosition;

    void LateUpdate()
    {
        HandleCameraCollision();
    }

void HandleCameraCollision()
{
    desiredCameraPosition = player.position - cameraHolder.forward * cameraDistance;

    Ray ray = new Ray(player.position, desiredCameraPosition - player.position);
    RaycastHit hit;

    Debug.DrawRay(ray.origin, ray.direction * cameraDistance, Color.red);

    if (Physics.Raycast(ray, out hit, cameraDistance, collisionLayers))
    {
        Vector3 hitPosition = hit.point + hit.normal * 0.5f;
        cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position, hitPosition, ref velocity, smoothTime);
    }
    else
    {
        cameraHolder.position = Vector3.SmoothDamp(cameraHolder.position, desiredCameraPosition, ref velocity, smoothTime);
    }
}

}
