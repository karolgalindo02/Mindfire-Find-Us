using UnityEngine;

public class PieceInteractable : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        // Assign the player's FPCamera
        mainCamera = GameObject.FindWithTag("FPCamera").GetComponent<Camera>();
    }

    private void OnMouseDown()
    {
        // Move the piece on click
        JigsawPuzzle.Instance.TryMovePiece(gameObject);
    }
}