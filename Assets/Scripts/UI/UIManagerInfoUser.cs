using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManagerInfoUser : MonoBehaviour
{
    [Header("UI Collected Items")]
    //Container of message info collected items
    [SerializeField] private GameObject UIMessageContainer;
    //Message
    [SerializeField] TextMeshProUGUI collectedItemUI;
    
    
    public void ShowMessage(string message)
    {
        StartCoroutine(DisplayMessage(message));
    }

    private IEnumerator DisplayMessage(string message)
    {
        collectedItemUI.text = message;

        UIMessageContainer.gameObject.SetActive(true);

        yield return new WaitForSeconds(2f);

        UIMessageContainer.gameObject.SetActive(false);
    }
}
