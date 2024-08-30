using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class PlayerInteractions : MonoBehaviour
{
    private GameObject currentItem;
    [SerializeField] private UIManagerInfoUser uiManagerInfoUser;
    [SerializeField] private GameObject uiPickUpItemContainer;
    [SerializeField] private TextMeshProUGUI uiPickUpItemMessage;
    [SerializeField] private KeyActivator keyActivator;

    [Header("Sounds")]
    [SerializeField] private AudioSource ammoSound;
    [SerializeField] private AudioSource knifeSound;
    [SerializeField] private AudioSource healthSound;
    [SerializeField] private AudioSource gunSound;
    [SerializeField] private AudioSource keySound;
    [SerializeField] private AudioSource fuseSound;
    [SerializeField] private AudioSource spiderWebSound;
    [SerializeField] private AudioSource pieceSound;
    [SerializeField] private AudioSource pencilSound;
    [SerializeField] public WeaponSwitch weaponSwitch;

    [Header("Paints Puzzle")]
    private HashSet<GameObject> checkedPaints = new HashSet<GameObject>();
    [SerializeField] private GameObject uiInfoPaintContainer;
    [SerializeField] private TextMeshProUGUI uiInfoPaintMessage;

    [Header("FuseBox Puzzle")]
    [SerializeField] private FusePuzzleController fusePuzzleController;
    [SerializeField] private MeshCollider fuseBoxCollider;
    [SerializeField] private GameObject fuseGreen;
    [SerializeField] private GameObject fuseBlue;
    [SerializeField] private GameObject fuseRed;

    public bool isfuseGreenActive = false;
    public bool isfuseBlueActive = false;
    public bool isfuseRedActive = false;
    [SerializeField] private CameraSwitch cameraSwitch;

    [Header("Raycast settings")]
    [SerializeField] private float firstPersonDistance = 2f;
    [SerializeField] private float thirdPersonDistance = 5f;
    [SerializeField] private RectTransform crosshair;


    public bool weaponCollected = false;
    public bool knifeCollected = false;
    public bool keyCollected = false;
    public bool fuseCollected = false;
    public bool keyBasementCollected = false;
    public bool pieceCollected = false;
    public bool pencilCollected = false;

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return; // Not interactable when the game is paused
        }
        if(cameraSwitch.isFirstPesonEnable && CameraSwitch.activeCamera.name == "FPCamera" || !cameraSwitch.isFirstPesonEnable && CameraSwitch.activeCamera.name == "ThirdPersonCamera_")
        {
            float rayDistance = cameraSwitch.isFirstPesonEnable ? firstPersonDistance : thirdPersonDistance;
           
            Vector3 crosshairWorldPostion = CameraSwitch.activeCamera.ScreenToViewportPoint(new Vector3(crosshair.position.x, crosshair.position.y, CameraSwitch.activeCamera.nearClipPlane));

            Ray ray = CameraSwitch.activeCamera.ScreenPointToRay(crosshair.position);

            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            if (Physics.Raycast(ray, out hit, rayDistance))
            {
                if (hit.collider.CompareTag("FuseBox"))
            {
                uiPickUpItemContainer.SetActive(true);
                uiPickUpItemMessage.text = "Press E to set fuse";

                if (Input.GetKeyDown(KeyCode.E))
                {
                    PlaceFuse();
                }
            }
            else if (hit.collider.CompareTag("Ammunition") || hit.collider.CompareTag("Weapon") || hit.collider.CompareTag("Knife") || hit.collider.CompareTag("Health") || hit.collider.CompareTag("Door") || hit.collider.CompareTag("Piece") || hit.collider.CompareTag("Pencil") || hit.collider.CompareTag("Paints") || hit.collider.CompareTag("DoorOpen") || hit.collider.CompareTag("Drawer") || hit.collider.CompareTag("Key") || hit.collider.CompareTag("Fuse") || hit.collider.CompareTag("SpiderWeb") || hit.collider.GetComponent<NavKeypad.Keypad>() != null)
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
                else if (hit.collider.CompareTag("DoorOpen"))
                {
                    uiPickUpItemMessage.text = "Press E to open";
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
                    uiPickUpItemMessage.text = "Press E to collect fuse";
                }

                else if (hit.collider.CompareTag("Piece"))
                {
                    uiPickUpItemMessage.text = "Press E to collect piece";
                }
                else if (hit.collider.CompareTag("Pencil"))
                {
                    uiPickUpItemMessage.text = "Press E to collect pencil";
                }
                else if (hit.collider.CompareTag("Paints"))
                {
                    PaintInteractable paintInteractable = hit.collider.GetComponent<PaintInteractable>();
                    if (paintInteractable != null && !checkedPaints.Contains(hit.collider.gameObject) && !PuzzleManager.Instance.IsPuzzleSolved())
                    {
                        uiPickUpItemContainer.SetActive(true);
                        uiPickUpItemMessage.text = "Press E to inspect the painting";

                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            StartCoroutine(ShowPaintInfoWithDelay(paintInteractable));
                        }
                    }
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
                    else if (hit.collider.CompareTag("DoorOpen"))
                    {
                        hit.collider.gameObject.GetComponent<AnimationDoor>().ChangeDoorState();
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
                        // Collect the fuse
                        CollectFuse(hit.collider.gameObject);
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
        }}
    }
    private IEnumerator ShowPaintInfoWithDelay(PaintInteractable paintInteractable)
    {
        yield return new WaitForSeconds(1); // Esperar 2 segundos

        uiPickUpItemContainer.SetActive(false); // Ocultar el mensaje de inspección
        uiInfoPaintContainer.SetActive(true);
        uiInfoPaintMessage.text = $"Painting: {paintInteractable.paintName}\nArtist: {paintInteractable.artistName}";

        PaintManager.Instance.SetCurrentPaintInfo(paintInteractable.paintName, paintInteractable.artistName);
        PuzzleManager.Instance.ShowPaintInfo(paintInteractable.paintName, paintInteractable.artistName);
    }

    private void CollectFuse(GameObject fuse)
    {
        fuseCollected = true;
        Destroy(fuse);
        fuseSound?.Play();
        uiManagerInfoUser.ShowMessage("Fuse collected");

        Inventory playerInventory = GetComponent<Inventory>();
        if (playerInventory != null)
        {
            Item fuseItem = fuse.GetComponent<Item>();
            if (fuseItem != null)
            {
                GameObject fuseCopy = Instantiate(fuse);
                fuseCopy.SetActive(false);
                playerInventory.AddItem(fuseCopy);
                weaponSwitch.UpdateUsableItems(); // Updates the usable elements
            }
        }
    }
    private void PlaceFuse()
    {
        if (fusePuzzleController != null)
        {
            // Check if the player has a fuse in hand
            bool hasFuseInHand = weaponSwitch.GetCurrentItemName() == "Fuse";

            if (hasFuseInHand)
            {
                // Check which fuse is not active and place it
                if (!isfuseGreenActive)
                {
                    fuseGreen.SetActive(true);
                    isfuseGreenActive = true;
                    weaponSwitch.RemoveItemFromInventory("Fuse", true);
                    weaponSwitch.SwitchToNextItem();
                }
                else if (!isfuseBlueActive)
                {
                    fuseBlue.SetActive(true);
                    isfuseBlueActive = true;
                    weaponSwitch.RemoveItemFromInventory("Fuse", true);
                    weaponSwitch.SwitchToNextItem();
                }
                else if (!isfuseRedActive)
                {
                    fuseRed.SetActive(true);
                    isfuseRedActive = true;
                    weaponSwitch.RemoveItemFromInventory("Fuse", true);
                    weaponSwitch.SwitchToNextItem();
                }
                else
                {
                    uiManagerInfoUser.ShowMessage("All fuses are already placed");
                }

                // Check if all fuses are placed
                if (isfuseGreenActive && isfuseBlueActive && isfuseRedActive)
                {
                    // All fuses are placed, disable convex on fuseBoxCollider
                    if (fuseBoxCollider != null)
                    {
                        fuseBoxCollider.convex = false;
                    }
                }
            }
            else
            {
                uiManagerInfoUser.ShowMessage("You need to have a fuse in hand to place it");
            }
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
                else if (itemComponent.itemName == "Piece")
                {
                    PuzzleManager.Instance.CollectPiece();
                    pieceCollected = true;
                }
                else if (itemComponent.itemName == "Pencil")
                {
                    pencilCollected = true;
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
        else if (currentItem.CompareTag("Piece") && pieceSound != null)
        {
            pieceSound.Play();
        }
        else if (currentItem.CompareTag("Pencil") && pencilSound != null)
        {
            pencilSound.Play();
        }
    }
}