using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> collectedItems = new List<GameObject>();
    [SerializeField] UIManagerInfoUser uiInfoUser;
  

    public void AddItem(GameObject item)
    {
        collectedItems.Add(item);
        Debug.Log($"{item.name} Has been added to inventory");

        uiInfoUser.ShowMessage($"{item.name} Has been added to inventory");
    }

  
}
