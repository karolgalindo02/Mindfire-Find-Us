using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveItem : MonoBehaviour
{
    public void OnInteract()
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}
