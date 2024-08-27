using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static PaintManager Instance;

    private string currentPaintName;
    private string currentArtistName;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentPaintInfo(string paintName, string artistName)
    {
        currentPaintName = paintName;
        currentArtistName = artistName;
    }

    public string GetCurrentPaintName()
    {
        return currentPaintName;
    }

    public string GetCurrentArtistName()
    {
        return currentArtistName;
    }
}