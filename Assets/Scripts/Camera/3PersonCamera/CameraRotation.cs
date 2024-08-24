using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float lookSpeed = 2.0f; // Velocidad de rotación vertical
    public float pitchMin = -80f; // Ángulo mínimo de inclinación
    public float pitchMax = 80f; // Ángulo máximo de inclinación

    private float pitch = 0.0f; // Ángulo de inclinación actual

    void Update()
    {
        // Obtener entrada del mouse para rotación vertical
        float mouseY = Input.GetAxis("Mouse Y");

        // Actualizar el ángulo de inclinación
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Aplicar la rotación vertical a la cámara
        transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, 0);
    }
}
