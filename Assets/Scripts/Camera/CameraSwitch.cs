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
    [SerializeField] private Transform tp_ItemPointFuse;
    [SerializeField] private Transform tp_ItemPointKey;
    [SerializeField] private Transform tp_ItemPointPiece;
    [SerializeField] private Transform tp_ItemPointPencil;
    [SerializeField] private Transform tp_ItemPointGun;
    [SerializeField] private Transform tp_ItemPointKnife;
    [SerializeField] private List<Transform> itemsToManage;
    [SerializeField] private LayerMask pieceMask;
    [SerializeField] private LayerMask pencilMask;
    [SerializeField] private LayerMask fuseMask;
    [SerializeField] private LayerMask keyMask;
    [SerializeField] private LayerMask gunMask;
    [SerializeField] private LayerMask knifeMask;
    public bool isGunActive = false;
    private Dictionary<Transform, Vector3> originalPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> originalRotations = new Dictionary<Transform, Quaternion>();



    private void Start()
    {
        activeCamera = firstPersonCamera;
        SaveCurrentTransform();

        foreach (Transform item in itemsToManage)
        {
            originalPositions[item] = item.localPosition;
            originalRotations[item] = item.localRotation;
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
        foreach (Transform item in itemsToManage)
        {
            Animator itemAnimator = item.GetComponent<Animator>();
            if (itemAnimator != null)
            {
                itemAnimator.enabled = false;
            }

            if (isFirstPesonEnable)
            {
                item.SetParent(fp_ParentCamera);
                item.localPosition = originalPositions[item];
                item.localRotation = originalRotations[item];
            }
            else
            {
                if ((pencilMask.value & (1 << item.gameObject.layer)) != 0)
                {
                    item.SetParent(tp_ItemPointPencil);
                }
                else if ((pieceMask.value & (1 << item.gameObject.layer)) != 0)
                {
                    item.SetParent(tp_ItemPointPiece);
                }
                else if ((fuseMask.value & (1 << item.gameObject.layer)) != 0)
                {
                    item.SetParent(tp_ItemPointFuse);

                }
                else if ((keyMask.value & (1 << item.gameObject.layer)) != 0)
                {
                    item.SetParent(tp_ItemPointKey);
                }
                else if((gunMask.value & (1 << item.gameObject.layer)) != 0){
                    
                    item.SetParent(tp_ItemPointGun); 
                    isGunActive=true;
                }
                else if((knifeMask.value & (1 << item.gameObject.layer)) != 0)
                {
                    item.SetParent(tp_ItemPointKnife);
                }
                
                item.localPosition = Vector3.zero;
                item.localRotation = Quaternion.identity;
            }

            if(itemAnimator != null)
            {
                itemAnimator.enabled = true;
            }
        }
    }
}
