using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{
    [SerializeField] private Inventory playerInventory;

    [SerializeField] private List<GameObject> weaponsInHierachy;

    [Header("Deactivated weapons")]
    // Reference to Gun deactivated
    [SerializeField] private GameObject inactiveWeapon;
    [SerializeField] private GameObject inactiveKnife;
    [SerializeField] private GameObject inactiveKey; // Referencia a la llave desactivada
    [SerializeField] private GameObject inactiveFuse; // Referencia al fusible desactivado

    // flag for check is gun is picked up
    [SerializeField] private PlayerInteractions playerInteractions;

    private int currentIndex = -1;

    private List<Item> usableItems;

    private void Start()
    {
        UpdateUsableItems();
    }

    public void UpdateUsableItems()
    {
        usableItems = playerInventory.GetUsableItems();
    }

    void Update()
    {
        if (Input.mouseScrollDelta.y != 0) 
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                currentIndex++;

                if (currentIndex >= usableItems.Count)
                {
                    currentIndex = -1; //withoutGun
                }
            }else if(Input.mouseScrollDelta.y < 0)
            {
                currentIndex--;

                if(currentIndex < -1)
                {
                    currentIndex = usableItems.Count - 1;
                }
            }

            SwitchWeapon();
        }
    }

    void SwitchWeapon()
    {
        // Desactivar todas las armas primero
        if (inactiveWeapon != null) inactiveWeapon.SetActive(false);
        if (inactiveKnife != null) inactiveKnife.SetActive(false);
        if (inactiveKey != null) inactiveKey.SetActive(false); // Desactivar la llave
        if (inactiveFuse != null) inactiveFuse.SetActive(false); // Desactivar el fusible

        if (currentIndex >= 0 && currentIndex < usableItems.Count)
        {
            Item currentItem = usableItems[currentIndex];

            if (currentItem.itemName == "Gun" && playerInteractions.weaponCollected)
            {
                // Gun activated
                inactiveWeapon.SetActive(true);
            }
            else if (currentItem.itemName == "Knife" && playerInteractions.knifeCollected)
            {
                // Knife activated
                inactiveKnife.SetActive(true);
            }
            else if (currentItem.itemName == "Key")
            {
                // Key activated
                inactiveKey.SetActive(true);
            }
            else if (currentItem.itemName == "Fuse")
            {
                // Fuse activated
                inactiveFuse.SetActive(true);
            }
        }
    }

    public void RemoveItemFromInventory(string itemName)
    {
        Item itemToRemove = usableItems.Find(item => item.itemName == itemName);
        if (itemToRemove != null)
        {
            usableItems.Remove(itemToRemove);
            playerInventory.RemoveItem(itemToRemove);
        }
    }
}