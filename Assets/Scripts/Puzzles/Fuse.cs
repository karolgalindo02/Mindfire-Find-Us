using UnityEngine;

public class Fuse : MonoBehaviour
{
    public bool IsLocked { get; private set; } = false;
    public bool IsCorrect { get; set; } = false;

    public void LockFuse()
    {
        IsLocked = true;
    }
}