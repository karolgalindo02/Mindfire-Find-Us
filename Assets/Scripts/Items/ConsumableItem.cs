using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumableItem : Item
{
    public int amount;

    private void Awake()
    {
        itemType = ItemType.Consumable;
    }
    public override void Use()
    {
        //Logic for consuming the item
        Debug.Log($"Consumated Item {itemName}");

        if(itemName == "Ammo")
        {
            GameManager.Instance.ammo += amount;
        }

        if(itemName == "Health")
        {
            GameManager.Instance.health += amount;
        }
        

        //Add specific logic to consume the item, such as increasing ammo or health
    }
}
