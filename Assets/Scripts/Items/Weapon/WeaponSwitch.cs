using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour
{

    [SerializeField] private Inventory playerInventory;

    //GameObject empty, point reference
    [SerializeField] private Transform weaponHolder;

    [SerializeField] private List<GameObject> weaponsInHierachy;

    //Reference to Gun deactivated
    [SerializeField] private GameObject inactiveWeapon;

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
        if (Input.mouseScrollDelta.y != 0 && playerInteractions.weaponCollected) 
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
        if(currentIndex >= 0 && currentIndex < usableItems.Count)
        {
            if (inactiveWeapon != null)
            {
                inactiveWeapon.SetActive(true);
                Debug.Log("Arma activada: " + inactiveWeapon.name);
            }
        }
        else if (currentIndex == -1 && inactiveWeapon != null)
        {
            inactiveWeapon.SetActive(false);
            Debug.Log("Arma desactivada");
        }
    }
}
