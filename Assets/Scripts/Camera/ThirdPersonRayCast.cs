using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonRayCast : MonoBehaviour
{
    [SerializeField] private Camera thirdPersonCamera;

    [SerializeField] private RectTransform crosshair;

    [SerializeField] private CameraSwitch cameraSwitch;

    [SerializeField] private LayerMask interactableMask;

    [SerializeField] private string healthTag = "Health";

    [SerializeField] private string ammunitionTag = "Ammunition";


    // Update is called once per frame
    void Update()
    {
        if (!cameraSwitch.isFirstPesonEnable)
        {
            ShootRaycast();
        }
        
    }

    private void ShootRaycast()
    {
        Vector3 crosshairWorldPostion = thirdPersonCamera.ScreenToViewportPoint(new Vector3(crosshair.position.x, crosshair.position.y, thirdPersonCamera.nearClipPlane));

        //Create a raycast from the camera to the crosshair position
        Ray ray = thirdPersonCamera.ScreenPointToRay(crosshair.position);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, interactableMask))
        {
            // Verificar si el objeto impactado tiene el tag específico
            if (hit.collider.CompareTag(healthTag) || hit.collider.CompareTag(ammunitionTag))
            {
                Debug.Log("Hit: " + hit.collider.name);
                
            }
        }
    

    }
}
