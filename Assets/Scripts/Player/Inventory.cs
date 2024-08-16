using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class Inventory : MonoBehaviour
{
    public List<GameObject> collectedItems = new List<GameObject>();
    //[SerializeField] UIManagerInfoUser uiInfoUser;
  

    public void AddItem(GameObject item)
    {
        collectedItems.Add(item);
        Debug.Log($"{item.name} Has been added to inventory");
       // uiInfoUser.ShowMessage($"{item.name} Has been added to inventory");
    }


    //Each Item must have the item component added
    public List<Item> GetUsableItems()
    {
        List<Item> usableItems = new List<Item>();

        foreach (GameObject item in collectedItems)
        {
            Item itemComponent = item.GetComponent<Item>();

            if (itemComponent != null && itemComponent.itemType == ItemType.Usable)
            {
                usableItems.Add(itemComponent);
            }
        }
        return usableItems;
    }

  
}
