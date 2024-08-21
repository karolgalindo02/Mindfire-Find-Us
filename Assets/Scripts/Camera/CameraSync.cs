using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    [SerializeField] private Transform firstPersonCamera; // First person camera
    [SerializeField] private Transform thirdPersonCamera; // Third person camera
    [SerializeField] CameraSwitch cameraSwitch;

    void LateUpdate()
    {
        UpdateCameraRotation();
    }

    private void UpdateCameraRotation()
    {
        if (!cameraSwitch.isFirstPesonEnable)
        {
            // Sinc rotation between First camera person and third camera person when the third camera is active
            Vector3 thirdPersonRotation = thirdPersonCamera.localEulerAngles;
            thirdPersonRotation.x = firstPersonCamera.localEulerAngles.x;
            thirdPersonCamera.localEulerAngles = thirdPersonRotation;
        }
    }
}
