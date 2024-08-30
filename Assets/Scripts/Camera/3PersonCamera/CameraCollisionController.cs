using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollisionController : MonoBehaviour
{
    public Transform player; // Reference point of the player.
    public Transform cameraHolder; // Container of the camera.
    public float cameraDistance = 2f; // Distance between the player and the camera.
    public LayerMask collisionLayers; // The layers that the camera can collide with..
    public float smoothSpeed = 10f; // Smoothing speed for interpolation.

    private Vector3 desiredCameraPosition;

    void LateUpdate()
    {
        HandleCameraCollision();
    }

    void HandleCameraCollision()
    {
        // Calculates the desired camera position
        desiredCameraPosition = player.position - cameraHolder.forward * cameraDistance;

        // Fires a ray from the player into the desired camera position
        Ray ray = new Ray(player.position, desiredCameraPosition - player.position);
        RaycastHit hit;

        //Debug Raycast
        Debug.DrawRay(ray.origin, ray.direction * cameraDistance, Color.red);

        if (Physics.Raycast(ray, out hit, cameraDistance, collisionLayers))
        {
            // If the ray collides with an object, it gently interpolates toward the point of collision
            Vector3 hitPosition = hit.point + hit.normal * 0.5f;
            cameraHolder.position = Vector3.Lerp(cameraHolder.position, hitPosition, smoothSpeed * Time.deltaTime);
        }
        else
        {
            // If there is no collision, gently interpolate to the desired position
            cameraHolder.position = Vector3.Lerp(cameraHolder.position, desiredCameraPosition, smoothSpeed * Time.deltaTime);
        }
    }

}
