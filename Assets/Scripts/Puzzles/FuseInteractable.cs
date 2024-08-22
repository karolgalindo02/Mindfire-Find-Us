using UnityEngine;

public class FuseInteractable : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;

    private void OnMouseDown()
    {
        isDragging = true;
        offset = transform.position - GetMouseWorldPos();
    }

    private void OnMouseUp()
    {
        isDragging = false;
        FusePuzzleController.Instance.MoveFuseToPosition(gameObject);
    }

    private void Update()
    {
        if (isDragging)
        {
            Vector3 mousePos = GetMouseWorldPos() + offset;
            transform.position = new Vector3(transform.position.x, mousePos.y, mousePos.z);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}