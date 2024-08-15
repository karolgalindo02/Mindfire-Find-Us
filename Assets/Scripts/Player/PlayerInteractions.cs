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



    private void Update()
    {
        if(IsNearItem && Input.GetKeyDown(KeyCode.E))
        {
            CollectItem();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammunition") || other.gameObject.CompareTag("Weapon") || other.gameObject.CompareTag("Knife"))
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
            //copy of the item
            GameObject itemCopy = Instantiate(currentItem);

            itemCopy.SetActive(false); //Deactivate the copy in the inventory

            playerInventory.AddItem(itemCopy);

            if (currentItem.CompareTag("Ammunition"))
            {
                //Here we add the number of variable ammo from the script AmunitionBox to the variable in the instance of the GamaManager
                GameManager.Instance.ammo += currentItem.GetComponent<AmmunitionBox>().ammo;
            }
            //Destroy item in scene
            Destroy(currentItem);
            //Hidde container PickUpInfo
            uiManagerInfoUser.HiddeCanvaElement(uiPickUpItemContainer);
            //Info user
            uiManagerInfoUser.ShowMessage($"{currentItem.name} Has been added to inventory");
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
}
