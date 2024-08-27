using UnityEngine;

public class PaintInteractable : MonoBehaviour
{
    public string paintName;
    public string artistName;

    private void OnMouseDown()
    {
        PaintManager.Instance.SetCurrentPaintInfo(paintName, artistName);
        PuzzleManager.Instance.ShowPaintInfo(paintName, artistName);
    }
}
