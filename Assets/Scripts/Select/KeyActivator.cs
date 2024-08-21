using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyActivator : MonoBehaviour
{
    [SerializeField] private GameObject keyBasement;

    public void ActivateKey()
    {
        if (keyBasement != null)
        {
            keyBasement.SetActive(true);
        }
    }
}