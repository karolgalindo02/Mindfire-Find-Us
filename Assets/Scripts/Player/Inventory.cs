using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<GameObject> collectedItems = new List<GameObject>();

    public void AddItem(GameObject item)
    {
        collectedItems.Add(item);
        Debug.Log($"{item.name} Has been added to inventory");
    }

    public void RemoveItem(Item item)
    {
        GameObject itemToRemove = collectedItems.Find(i => i.GetComponent<Item>() == item);
        if (itemToRemove != null)
        {
            collectedItems.Remove(itemToRemove);
            Destroy(itemToRemove);
            Debug.Log($"{item.name} Has been removed from inventory");
        }
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