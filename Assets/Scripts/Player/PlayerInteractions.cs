using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private bool IsNearItem = false;

    private GameObject currentItem;

    [SerializeField] private UIManagerInfoUser uiManagerInfoUser;

    //Container of the message for pick up items
    [SerializeField] private GameObject uiPickUpItemContainer;

    // Reference to script for change weapon
    [SerializeField] private WeaponSwitch weaponSwitch;


    //Check if weapon is picked up
    public bool weaponCollected = false;

    //Check if knife is picked up
    public bool knifeCollected = false;


    private void Update()
    {
        if(IsNearItem && Input.GetKeyDown(KeyCode.E))
        {            
            PlaySoundForCurrentItem();
            CollectItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammunition") || other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Knife") || other.gameObject.CompareTag("Health"))
        {
            IsNearItem = true;
            currentItem = other.gameObject;
            //uiManagerInfoUser.ShowMessage("Press E to pick up item");
            uiPickUpItemContainer.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == currentItem)
        {
            IsNearItem = false;
            currentItem = null;
            uiPickUpItemContainer.gameObject.SetActive(false);
        }
    }

    private void CollectItem()
    {
        //For use the inventory of the player
        Inventory playerInventory = GetComponent<Inventory>();

        if (playerInventory != null && currentItem != null)
        {
            //We saved the component of the gameObject
            Item itemComponent = currentItem.GetComponent<Item>();

            if (itemComponent != null) {

                //copy of the item
                GameObject itemCopy = Instantiate(currentItem);

                itemCopy.SetActive(false); //Deactivate the copy in the inventory

                playerInventory.AddItem(itemCopy);

                if(itemComponent.itemType == ItemType.Consumable)
                {
                    //use the item inmediatly
                    itemComponent.Use();
                    //We hidde container of the message "Press E"
                    uiManagerInfoUser.HiddeCanvaElement(uiPickUpItemContainer);
                }
                else
                {
                    //Hidde container PickUpInfo
                    uiManagerInfoUser.HiddeCanvaElement(uiPickUpItemContainer);
                    //Info user
                    uiManagerInfoUser.ShowMessage($"{currentItem.name} Has been added to inventory");

                    //itemComponent.Use();
                }

                if(itemComponent.itemName == "Gun")
                {
                    weaponCollected = true;
                }else if (itemComponent.itemName == "Knife")
                {
                    knifeCollected = true;
                }

                //Destroy the item at scene
                Destroy(currentItem);
                // Actualizar la lista de �tems usables en WeaponSwitch
                weaponSwitch.UpdateUsableItems();
            }
           
        }

        IsNearItem = false;

        currentItem = null;
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.LoseHealth(5);
        }
    }
    private void PlaySoundForCurrentItem()
{
    if (currentItem == null) return;

    if (currentItem.CompareTag("Weapon") && weaponSound != null)
    {
        weaponSound.Play();
    }
    else if (currentItem.CompareTag("Knife") && knifeSound != null)
    {
        knifeSound.Play();
    }
    else if (currentItem.CompareTag("Ammunition") && ammoSound != null)
    {
        ammoSound.Play();
    }
}
}
