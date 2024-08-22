using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSync : MonoBehaviour
{
    [SerializeField] private Transform firstPersonCamera; // First person camera
    [SerializeField] private Transform thirdPersonCamera; // Third person camera
    [SerializeField] CameraSwitch cameraSwitch;
    [SerializeField] private float minVerticalAngle = -30f; // Lower limit of vertical rotation
    [SerializeField] private float maxVerticalAngle = 60f; // Upper limit of vertical rotation


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

            // Convert the angle to a range of -180 to 180 degrees to apply the limits correctly
            if (thirdPersonRotation.x> 180)
            {
                thirdPersonRotation.x -= 360;
            }

            // Enforce vertical rotation limits
            thirdPersonRotation.x = Mathf.Clamp(thirdPersonRotation.x, minVerticalAngle, maxVerticalAngle);
            
            //Convert the angle back to a range of 0 to 360 degrees
            if (thirdPersonRotation.x < 0)
            {
                thirdPersonRotation.x += 360;
            }

            thirdPersonCamera.localEulerAngles = thirdPersonRotation;
        }
    }
}
