using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerInteractions : MonoBehaviour
{
    private GameObject currentItem;
    [SerializeField] private UIManagerInfoUser uiManagerInfoUser;
    [SerializeField] private GameObject uiPickUpItemContainer;
    [SerializeField] private TextMeshProUGUI uiPickUpItemMessage;

    [Header("Sounds")]
    [SerializeField] private AudioSource ammoSound;
    [SerializeField] private AudioSource knifeSound;
    [SerializeField] private AudioSource healthSound;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private AudioSource keySound;
    [SerializeField] private AudioSource fuseSound;
    [SerializeField] private AudioSource spiderWebSound;
    [SerializeField] public WeaponSwitch weaponSwitch;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private KeyActivator keyActivator;
    [SerializeField] private FusePuzzleController fusePuzzleController; // Referencia al controlador de fusibles

    public bool weaponCollected = false;
    public bool knifeCollected = false;
    public bool keyCollected = false;
    public bool fuseCollected = false;
    public bool keyBasementCollected = false;

    private void Update()
    {
    
        Ray ray = CameraSwitch.activeCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 2f))
        {
            if (hit.collider.CompareTag("Ammunition") || hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Knife") || hit.collider.CompareTag("Health") || hit.collider.CompareTag("Door") || hit.collider.CompareTag("Drawer") || hit.collider.CompareTag("Key") || hit.collider.CompareTag("Fuse") || hit.collider.CompareTag("SpiderWeb") || hit.collider.GetComponent<NavKeypad.Keypad>() != null)
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

                if (hit.collider.CompareTag("Door"))
                {
                    bool hasKey = keyCollected || keyBasementCollected;
                    bool isDoorOpen = hit.collider.gameObject.GetComponent<SystemDoor>().IsDoorOpen;
                    bool isDoorUnlocked = hit.collider.gameObject.GetComponent<SystemDoor>().IsDoorUnlocked;

                    if (isDoorOpen || isDoorUnlocked)
                    {
                        uiPickUpItemMessage.text = "Press E to open";
                    }
                    else if (hasKey)
                    {
                        uiPickUpItemMessage.text = "Use the key to open the door";
                    }
                    else
                    {
                        uiPickUpItemMessage.text = "Door is locked. You need a key to open it.";
                    }
                }
                else if (hit.collider.CompareTag("Drawer"))
                {
                    uiPickUpItemMessage.text = "Press E to open";
                }
                else if (hit.collider.CompareTag("SpiderWeb"))
                {
                    uiPickUpItemMessage.text = "Press E to remove web";
                }
                else if (hit.collider.GetComponent<NavKeypad.Keypad>() != null)
                {
                    uiPickUpItemMessage.text = "Press E to interact with keypad";
                }
                else if (hit.collider.CompareTag("Fuse"))
                {
                    uiPickUpItemMessage.text = "Press E to interact with fuse";
                }
                else
                {
                    uiPickUpItemMessage.text = "Press E to pick up item";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (hit.collider.CompareTag("Door"))
                    {
                        bool hasKey = keyCollected || keyBasementCollected;
                        bool keyInHand = weaponSwitch.GetCurrentItemName() == "Key" || weaponSwitch.GetCurrentItemName() == "KeyBasement";
                        string keyName = weaponSwitch.GetCurrentItemName();
                        bool correctDoor = hit.collider.gameObject.GetComponent<SystemDoor>().ChangeDoorState(hasKey, keyInHand, keyName);

                        if (hasKey && keyInHand && correctDoor)
                        {
                            if (keyName == "Key")
                            {
                                keyCollected = false;
                                weaponSwitch.RemoveItemFromInventory("Key", true);
                                weaponSwitch.SwitchToNextItem();
                                uiPickUpItemMessage.text = "Press E to open";
                                keyActivator.ActivateKey();
                            }
                            else if (keyName == "KeyBasement")
                            {
                                keyBasementCollected = false;
                                weaponSwitch.RemoveItemFromInventory("KeyBasement", true);
                                weaponSwitch.SwitchToNextItem();
                                uiPickUpItemMessage.text = "Press E to open";
                            }
                        }
                    }
                    else if (hit.collider.CompareTag("Drawer"))
                    {
                        hit.collider.gameObject.GetComponent<SystemDrawer>().ChangeDrawerState();
                    }
                    else if (hit.collider.CompareTag("SpiderWeb"))
                    {
                        if (spiderWebSound != null)
                        {
                            spiderWebSound.Play();
                        }
                        Destroy(hit.collider.gameObject);
                    }
                    else if (hit.collider.GetComponent<NavKeypad.Keypad>() != null)
                    {
                        // Interact with the keypad
                        hit.collider.GetComponent<NavKeypad.Keypad>().AddInput("enter");
                    }
                    else if (hit.collider.CompareTag("Fuse"))
                    {
                        // Interact with the fuse
                        FuseInteractable fuseInteractable = hit.collider.GetComponent<FuseInteractable>();
                        if (fuseInteractable != null)
                        {
                            // Aquí puedes agregar lógica adicional si es necesario
                        }
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
                else if (itemComponent.itemName == "KeyBasement")
                {
                    keyBasementCollected = true;
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
        else if (currentItem.CompareTag("SpiderWeb") && spiderWebSound != null)
        {
            spiderWebSound.Play();
        }
    }
}