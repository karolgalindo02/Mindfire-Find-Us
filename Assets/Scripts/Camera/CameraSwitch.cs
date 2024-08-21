using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] bool isFirstPesonEnable = true;


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
           isFirstPesonEnable = !isFirstPesonEnable;
           ChangeCamera();
        }
    }

    private void ChangeCamera()
    {
        if (isFirstPesonEnable)
        {
            firstPersonCamera.enabled = true;
            thirdPersonCamera.enabled = false;
        }
        else
        {
            firstPersonCamera.enabled = false;
            thirdPersonCamera.enabled = true;
        }
        
    }
}
