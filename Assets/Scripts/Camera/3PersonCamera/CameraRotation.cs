using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    public float lookSpeed = 2.0f; // Velocidad de rotaci�n vertical
    public float pitchMin = -80f; // �ngulo m�nimo de inclinaci�n
    public float pitchMax = 80f; // �ngulo m�ximo de inclinaci�n

    private float pitch = 0.0f; // �ngulo de inclinaci�n actual

    void Update()
    {
        // Obtener entrada del mouse para rotaci�n vertical
        float mouseY = Input.GetAxis("Mouse Y");

        // Actualizar el �ngulo de inclinaci�n
        pitch -= mouseY * lookSpeed;
        pitch = Mathf.Clamp(pitch, pitchMin, pitchMax);

        // Aplicar la rotaci�n vertical a la c�mara
        transform.localRotation = Quaternion.Euler(pitch, transform.localRotation.eulerAngles.y, 0);
    }
}
