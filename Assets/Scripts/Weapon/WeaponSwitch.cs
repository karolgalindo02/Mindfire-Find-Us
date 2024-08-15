using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    [SerializeField] private Inventory playerInventory;

    [SerializeField] private Transform weaponHolder;

    private int currentIndex = 0;
    
    
    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) 
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                currentIndex++;

                if (currentIndex >= playerInventory.collectedItems.Count)
                {
                    currentIndex = 0;
                }
            }else if(Input.mouseScrollDelta.y < 0)
            {
                currentIndex--;
                if(currentIndex < 0)
                {
                    currentIndex = playerInventory.collectedItems.Count - 1;
                }
            }

            SwitchWeapon();
        }

    }

    void SwitchWeapon()
    {
        foreach (GameObject item in playerInventory.collectedItems)
        {
            item.SetActive(false);
        }

        if (playerInventory.collectedItems.Count > 0)
        {
            GameObject currentItem = playerInventory.collectedItems[currentIndex];
            currentItem.SetActive(true);
            currentItem.transform.position = weaponHolder.position;
            currentItem.transform.rotation = weaponHolder.rotation;
        }
    }
}
