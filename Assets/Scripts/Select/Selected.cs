using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private PlayerInteractions playerInteractions;

    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance))
        {
            if (hit.collider.tag == "Door")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    bool hasKey = playerInteractions.keyCollected;
                    bool keyInHand = playerInteractions.weaponSwitch.GetCurrentItemName() == "Key";
                    string keyName = playerInteractions.weaponSwitch.GetCurrentItemName();
                    SystemDoor door = hit.collider.gameObject.GetComponent<SystemDoor>();
                    door.ChangeDoorState(hasKey, keyInHand, keyName);

                    if (hasKey && keyInHand && door.IsDoorUnlocked)
                    {
                        playerInteractions.keyCollected = false;
                        playerInteractions.weaponSwitch.RemoveItemFromInventory("Key", true);
                        playerInteractions.weaponSwitch.SwitchToNextItem();
                    }
                    else if (hasKey && keyInHand && !door.IsDoorUnlocked)
                    {
                        // playerInteractions.weaponSwitch.RemoveItemFromInventory("Key", false);
                    }
                }
            }
        }
    }
}