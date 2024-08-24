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

    public float interactionDistance = 10f; // Distancia del Raycast de la mira


    // Update is called once per frame
    void Update()
    {
        //When the camera change position we block the raycast of the player en third person Camera
        /*
        if (!cameraSwitch.isFirstPesonEnable && thirdPersonCamera.transform.position.z > -1.756f)
        {
            
        }
        */

        ShootRaycast();

    }

    private void ShootRaycast()
    {
        Vector3 crosshairWorldPostion = thirdPersonCamera.ScreenToViewportPoint(new Vector3(crosshair.position.x, crosshair.position.y, thirdPersonCamera.nearClipPlane));

        //Create a raycast from the camera to the crosshair position
        Ray ray = thirdPersonCamera.ScreenPointToRay(crosshair.position);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * interactionDistance, Color.green);

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
