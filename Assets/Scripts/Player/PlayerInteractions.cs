using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private GameObject currentItem;

    [SerializeField] private UIManagerInfoUser uiManagerInfoUser;
    [SerializeField] private GameObject uiPickUpItemContainer;
    [SerializeField] private AudioSource ammoSound;
    [SerializeField] private AudioSource knifeSound;
    [SerializeField] private AudioSource healthSound;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private AudioSource keySound;
    [SerializeField] private AudioSource fuseSound;
    [SerializeField] public WeaponSwitch weaponSwitch;
    [SerializeField] private Camera mainCamera;

    public bool weaponCollected = false;
    public bool knifeCollected = false;
    public bool keyCollected = false;
    public bool fuseCollected = false;

    private void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.CompareTag("Ammunition") || hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Knife") || hit.collider.CompareTag("Health") || hit.collider.CompareTag("Door") || hit.collider.CompareTag("Drawer") || hit.collider.CompareTag("Key") || hit.collider.CompareTag("Fuse"))
            {
                if (currentItem != hit.collider.gameObject)
                {
                    if (currentItem != null)
                    {
                        if (currentItem.CompareTag("Weapon"))
                        {
                            foreach (Renderer renderer in currentItem.GetComponentsInChildren<Renderer>())
                            {
                                renderer.material.DisableKeyword("_EMISSION");
                            }
                        }
                        else
                        {
                            currentItem.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                        }
                    }

                    currentItem = hit.collider.gameObject;
                    if (currentItem.CompareTag("Weapon"))
                    {
                        foreach (Renderer renderer in currentItem.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material.EnableKeyword("_EMISSION");
                        }
                    }
                    else
                    {
                        currentItem.GetComponent<Renderer>().material.EnableKeyword("_EMISSION");
                    }
                }

                uiPickUpItemContainer.gameObject.SetActive(true);

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.CompareTag("Door"))
                    {
                        bool hasKey = keyCollected;
                        hit.collider.gameObject.GetComponent<SystemDoor>().ChangeDoorState(hasKey);

                        if (hasKey)
                        {
                            keyCollected = false;
                            weaponSwitch.RemoveItemFromInventory("Key");
                        }
                    }
                    else if (hit.collider.CompareTag("Drawer"))
                    {
                        hit.collider.gameObject.GetComponent<SystemDrawer>().ChangeDrawerState();
                    }
                    else
                    {
                        PlaySoundForCurrentItem();
                        CollectItem();
                    }
                }
            }
            else
            {
                if (currentItem != null)
                {
                    if (currentItem.CompareTag("Weapon"))
                    {
                        foreach (Renderer renderer in currentItem.GetComponentsInChildren<Renderer>())
                        {
                            renderer.material.DisableKeyword("_EMISSION");
                        }
                    }
                    else
                    {
                        currentItem.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                    }
                    currentItem = null;
                }
                uiPickUpItemContainer.gameObject.SetActive(false);
            }
        }
        else
        {
            if (currentItem != null)
            {
                if (currentItem.CompareTag("Weapon"))
                {
                    foreach (Renderer renderer in currentItem.GetComponentsInChildren<Renderer>())
                    {
                        renderer.material.DisableKeyword("_EMISSION");
                    }
                }
                else
                {
                    currentItem.GetComponent<Renderer>().material.DisableKeyword("_EMISSION");
                }
                currentItem = null;
            }
            uiPickUpItemContainer.gameObject.SetActive(false);
        }
    }

    private void CollectItem()
    {
        Inventory playerInventory = GetComponent<Inventory>();

        if (playerInventory != null && currentItem != null)
        {
            Item itemComponent = currentItem.GetComponent<Item>();

            if (itemComponent != null)
            {
                GameObject itemCopy = Instantiate(currentItem);
                itemCopy.SetActive(false);
                playerInventory.AddItem(itemCopy);

                if (itemComponent.itemType == ItemType.Consumable)
                {
                    itemComponent.Use();
                    uiManagerInfoUser.HiddeCanvaElement(uiPickUpItemContainer);
                }
                else
                {
                    uiManagerInfoUser.HiddeCanvaElement(uiPickUpItemContainer);
                    uiManagerInfoUser.ShowMessage($"{currentItem.name} Has been added to inventory");
                }

                if (itemComponent.itemName == "Gun")
                {
                    weaponCollected = true;
                }
                else if (itemComponent.itemName == "Knife")
                {
                    knifeCollected = true;
                }
                else if (itemComponent.itemName == "Key")
                {
                    keyCollected = true;
                }
                else if (itemComponent.itemName == "Fuse")
                {
                    fuseCollected = true;
                }

                Destroy(currentItem);
                weaponSwitch.UpdateUsableItems();
            }
        }

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

        if (currentItem.CompareTag("Weapon") && gunSound != null)
        {
            gunSound.Play();
        }
        else if (currentItem.CompareTag("Knife") && knifeSound != null)
        {
            knifeSound.Play();
        }
        else if (currentItem.CompareTag("Ammunition") && ammoSound != null)
        {
            ammoSound.Play();
        }
        else if (currentItem.CompareTag("Health") && healthSound != null)
        {
            healthSound.Play();
        }
        else if (currentItem.CompareTag("Key") && keySound != null)
        {
            keySound.Play();
        }
        else if (currentItem.CompareTag("Fuse") && fuseSound != null)
        {
            fuseSound.Play();
        }
    }
}