using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    [SerializeField] private float distance = 3f;
    [SerializeField] private PlayerInteractions playerInteractions;

    void Update()
    {
        //Raycast(origin, direction, hitInfo, distance, layerMask)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance))
        {
            if (hit.collider.tag == "Door")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    bool hasKey = playerInteractions.keyCollected;
                    hit.collider.gameObject.GetComponent<SystemDoor>().ChangeDoorState(hasKey);

                    if (hasKey)
                    {
                        playerInteractions.keyCollected = false;
                        playerInteractions.weaponSwitch.RemoveItemFromInventory("Key");
                    }
                }
            }
        }
    }
}