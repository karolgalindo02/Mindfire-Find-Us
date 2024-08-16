using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsableItem : Item
{
    private void Awake()
    {
        itemType = ItemType.Usable;
    }

    public override void Use()
    {
        Debug.Log(itemName + " equipado en el WeaponHolder");

        //Optimize for sprint 2
    }
}
