using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwitch : MonoBehaviour
{
    [Header("Cameras")]
    [SerializeField] private Camera thirdPersonCamera;
    [SerializeField] private Camera child3PCamera;
    [SerializeField] private Camera firstPersonCamera;
    [SerializeField] private Camera childFPCamera;


    [SerializeField] public bool isFirstPesonEnable = true;
    [SerializeField] private CameraLook cameraLookScript; // Reference to the CameraLook script
    [SerializeField] public int gameMode = 1; //Can be 1 or 3 first person and third person mode
    public static Camera activeCamera;

    private Vector3 lastPlayerPosition;
    private Quaternion lastPlayerRotation;

    [Header("Items usables position")]
    [SerializeField] private Transform fp_ItemUsableMount;
    [SerializeField] private Transform tp_ItemUsableMount;
    [SerializeField] private Transform weaponPositionTest;
    


    private void Start()
    {
        activeCamera = firstPersonCamera;
        SaveCurrentTransform();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
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
            //child3PCamera.enabled = false;
            activeCamera = firstPersonCamera;
            gameMode = 1;
        }
        else
        {
            firstPersonCamera.enabled = false;
            childFPCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            //child3PCamera.enabled = true;
            activeCamera = thirdPersonCamera;
            gameMode = 3;
           
        }

        RestoreLastTransform();
        
    }
    private void UpdateItemsPosition()
    {
        if (isFirstPesonEnable)
        {
            weaponPositionTest.SetParent(fp_ItemUsableMount);
            weaponPositionTest.localPosition = Vector3.zero;
            weaponPositionTest.localRotation = Quaternion.identity;

        }
        else
        {
            weaponPositionTest.SetParent(tp_ItemUsableMount);
            weaponPositionTest.localPosition = Vector3.zero;
            weaponPositionTest.localRotation = Quaternion.identity;
        }
    }
}
