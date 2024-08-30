using UnityEngine;

public class PieceInteractable : MonoBehaviour
{
    private Camera mainCamera;
    private AudioSource slideSound;

    private void Start()
    {
        mainCamera = GameObject.FindWithTag("FPCamera").GetComponent<Camera>();
        GameObject slideSoundObject = GameObject.Find("SlideSound");
        if (slideSoundObject != null)
        {
            slideSound = slideSoundObject.GetComponent<AudioSource>();
        }
    }

    private void OnMouseDown()
    {
        JigsawPuzzle.Instance.TryMovePiece(gameObject);
        if (slideSound != null)
        {
            slideSound.Play();
        }
    }
}