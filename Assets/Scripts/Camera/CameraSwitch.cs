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
    [SerializeField] private Transform fp_ParentCamera;
    [SerializeField] private Transform tp_ItemUsableMount;
    [SerializeField] private List<Transform> itemsToManage;
    //[SerializeField] private Transform initialWeaponPosition;
    [SerializeField] private List<Vector3> originalItemPositions;
    [SerializeField] private List<Quaternion> originalItemRotations;
    


    private void Start()
    {
        activeCamera = firstPersonCamera;
        SaveCurrentTransform();
        originalItemPositions = new List<Vector3>();
        originalItemRotations = new List<Quaternion>();
        /*
        originalWeaponPosition = initialWeaponPosition.localPosition;
        originalWeaponRotation = initialWeaponPosition.localRotation;
        */

        foreach(Transform item in itemsToManage)
        {
            originalItemPositions.Add(item.localPosition);
            originalItemRotations.Add(item.localRotation);
        }

    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
           isFirstPesonEnable = !isFirstPesonEnable;
           ChangeCamera();
           UpdateItemsPosition();
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
            gameMode = 1;
        }
        else
        {
            firstPersonCamera.enabled = false;
            childFPCamera.enabled = false;
            thirdPersonCamera.enabled = true;
            child3PCamera.enabled = true;
            activeCamera = thirdPersonCamera;
            gameMode = 3;
           
        }

        RestoreLastTransform();
        
    }
    private void UpdateItemsPosition()
    {
        /*
        if (isFirstPesonEnable)
        {
            initialWeaponPosition.SetParent(fp_ParentCamera);
            initialWeaponPosition.localPosition = originalWeaponPosition;
            initialWeaponPosition.localRotation = originalWeaponRotation;
        }
        else
        {
            initialWeaponPosition.SetParent(tp_ItemUsableMount);
            initialWeaponPosition.localPosition = Vector3.zero;
            initialWeaponPosition.localRotation = Quaternion.identity;
        }
        */

        for (int i = 0; i < itemsToManage.Count; i++)
        {
            if (isFirstPesonEnable)
            {
                itemsToManage[i].SetParent(fp_ParentCamera);
                itemsToManage[i].localPosition = originalItemPositions[i];
                itemsToManage[i].localRotation = originalItemRotations[i];
            }
            else
            {
                itemsToManage[i].SetParent(tp_ItemUsableMount);
                itemsToManage[i].localPosition = Vector3.zero;
                itemsToManage[i].localRotation = Quaternion.identity;
            }
        }

    }
}
