using UnityEngine;

public class FuseInteractable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    [SerializeField] private Camera mainCamera;

    // The mouse button is pressed
    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
        EnableEmission();
    }

    // The mouse button is released
    private void OnMouseUp()
    {
        isDragging = false;
        FusePuzzleController.Instance.CheckFusePosition(gameObject);

        // Only disable emission if the fuse is not locked in the correct position
        Fuse fuseScript = GetComponent<Fuse>();
        if (fuseScript != null && !fuseScript.IsLocked)
        {
            DisableEmission();
        }
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos() + offset;
            transform.position = new Vector3(transform.position.x, mousePos.y, mousePos.z);
        }
    }

    // Position of the mouse in the world
    private Vector3 GetMouseWorldPos()
    {
        if (mainCamera == null)
        {
            return Vector3.zero;
        }

        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mainCamera.WorldToScreenPoint(transform.position).z;
        return mainCamera.ScreenToWorldPoint(mousePoint);
    }

    // Activate material emission
    private void EnableEmission()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.EnableKeyword("_EMISSION");
        }
    }

    // Desactivate material emission
    private void DisableEmission()
    {
        Renderer renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.DisableKeyword("_EMISSION");
        }
    }
}