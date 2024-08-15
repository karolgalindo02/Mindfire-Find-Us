using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammunition"))
        {
            //For use the inventory of the player
            Inventory playerInventory = GetComponent<Inventory>();
            //Here we add the number of variable ammo from the script AmunitionBox to the variable in the instance of the GamaManager
            GameManager.Instance.ammo += other.gameObject.GetComponent<AmmunitionBox>().ammo;
            
            if(playerInventory != null)
            {
                //We add our object in the inventory
                playerInventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }

        }

        if (other.gameObject.CompareTag("Weapon"))
        {
            Inventory playerInventory = GetComponent<Inventory>();

            if (playerInventory != null)
            {
                //We add our object in the inventory
                playerInventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }

        if (other.gameObject.CompareTag("Knife"))
        {
            Inventory playerInventory = GetComponent<Inventory>();

            if (playerInventory != null)
            {
                //We add our object in the inventory
                playerInventory.AddItem(other.gameObject);
                other.gameObject.SetActive(false);
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("EnemyBullet"))
        {
            GameManager.Instance.LoseHealth(5);
        }
    }
}
