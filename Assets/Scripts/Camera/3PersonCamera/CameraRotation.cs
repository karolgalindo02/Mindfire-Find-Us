using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float lookSpeed = 2.0f; // Vertical rotation speed
    public float angleMin = -80f; // Minimum tilt angle
    public float angleMax = 80f; // Maximum tilt angle

    private float pitch = 0.0f; // Current Tilt Angle

    void Update()
    {
        // Get mouse input for vertical rotation
        float mouseY = Input.GetAxis("Mouse Y");

        // Update the tilt angle
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, angleMin, angleMax);

        // Apply vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, 0);
    }
}
