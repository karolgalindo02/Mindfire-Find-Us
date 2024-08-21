using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera child3PCamera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera childFPCamera;
    [SerializeField] public bool isFirstPesonEnable = true;
    [SerializeField] private CameraLook cameraLookScript; // Reference to the CameraLook script
    public static Camera activeCamera;

    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;
   
    

    private void Start()
    {
        activeCamera = firstPersonCamera;
        SaveCurrentTransform();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
           isFirstPesonEnable = !isFirstPesonEnable;
           ChangeCamera();
        }
    }

    private void SaveCurrentTransform()
    {
        lastPlayerPosition = transform.position;
        lastPlayerRotation = transform.rotation;
    }

    public void RestoreLastTransform()
    {
        transform.position = lastPlayerPosition;
        transform.rotation = lastPlayerRotation;
    }

    private void ChangeCamera()
    {
        SaveCurrentTransform();

        if (isFirstPesonEnable)
        {
            firstPersonCamera.enabled = true;
            childFPCamera.enabled = true;
            thirdPersonCamera.enabled = false;
            child3PCamera.enabled = false;
            activeCamera = firstPersonCamera;
        }
        else
        {
            firstPersonCamera.enabled = false;
            childFPCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            child3PCamera.enabled = true;
            activeCamera = thirdPersonCamera;
           
        }

        RestoreLastTransform();
        
    }
}
