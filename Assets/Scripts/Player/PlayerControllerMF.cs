using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.Playables;
using UnityEngine.UIElements;

public class PlayerControllerMF : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] private float speedMove = 4.0f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource steps;
    [SerializeField] private Transform playerBody;
    [SerializeField] private Transform cameraHolderFP; 
    [SerializeField] private Transform cameraHolderTP;
    [SerializeField] private float rotationSpeed = 80f;

    [Header("Activate rotation 3P")]
    //for limit the rotation between cameras
    [SerializeField] CameraSwitch cameraSwitch;
   


    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed = 3.0f; // Crouching Speed
    [SerializeField] private float crouchHeight = 1.0f; // Height when crouching
    [SerializeField] private float standingHeight = 1.7f; // Standing height
    [SerializeField] private Vector3 crouchCameraPositionOffsetFP = new Vector3(0, -0.5f, 0); 
    [SerializeField] private Vector3 standingCameraPositionOffsetFP = new Vector3(0, 0.0f, 0);
    [SerializeField] private Vector3 crouchCameraPositionOffsetTP = new Vector3(0, -0.5f, 0); 
    [SerializeField] private Vector3 standingCameraPositionOffsetTP = new Vector3(0, 0.0f, 0); 
    private float originalCenterY; // Original Y center of the CharacterController
    private Vector3 originalCameraLocalPositionFP;
    private Vector3 originalCameraLocalPositionTP;
    //[Header("Canvas")]

  
    [Header("Gravity settings")]
    //Because we dont use RigidBody
    [SerializeField]private float gravity = -9.81f;
    //This is the position of the gameObject empty for check is our player touched the floor
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float shpereRadius = 0.3f;
    //Mask that we created for our scene to defined what is or what is not floor
    [SerializeField] private LayerMask groundMask;
    //Variable to saved the force that we apply in the Axis "Y" for simulated the gravity
    private Vector3 velocity;

    private Vector3 moveDirection = Vector3.zero;

    [Header("States")]
    //Variable that is configured with the empty object that we set under the Player to check whether or not it is touching the ground.
    [SerializeField] public bool isGrounded, isMoving, isGameOver;
    [SerializeField] private bool isCrouching = false;
    public bool isDead = false;

    [Header("Animations")]
    Vector3 moveDirectionWithCamera3P;
    Transform cameraObject;
    public Animator animator;
    public float moveHorizontal, moveVertical;
        
    [Header("Weapon")]
    [SerializeField] private GameObject gun;
    [SerializeField] private GameObject knife;
    [SerializeField] private bool isGunActive = false;
    [SerializeField] private bool isKnifeActive = false;
    private bool isThirdPersonView;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator =GetComponent<Animator>();
        originalCenterY = controller.center.y;
        originalCameraLocalPositionFP = cameraHolderFP.localPosition;
        originalCameraLocalPositionTP = cameraHolderTP.localPosition;    
        isGunActive = gun.activeSelf;
        isKnifeActive = knife.activeSelf;
    }

    void Update()
    {
        HandleMovement();
        HandleCrouch();
        AdjustCameraPosition();
        CheckGunStatus();
        CheckKnifeStatus();
        CheckViewStatus();
        UpdateViewStatus();
        HandleAimingAndCrouching();
        HandleInput();

    }
     void CheckKnifeStatus()
    {
        // Actualiza el estado de si el cuchillo está activo o no
        isKnifeActive = knife.activeSelf;
    }    void UpdateViewStatus()
    {
        isThirdPersonView = !cameraSwitch.isFirstPesonEnable;
    }
    void CheckViewStatus()
    {
        // Actualizar el estado de la vista en cada actualización
        isThirdPersonView = !cameraSwitch.isFirstPesonEnable;
    }
    void CheckGunStatus()
    {
        isGunActive = gun.activeSelf;
    }

    void HandleMovement()
    {
        if (Time.timeScale==0) return;
        isGrounded = Physics.CheckSphere(groundCheck.position, shpereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            // Ensure that the player is well aligned with the ground
            velocity.y = -2f;
        }

        // Get mouse input
        float mouseX = Input.GetAxis("Mouse X");

        float mouseY = Input.GetAxis("Mouse Y");

        // Get keyboard input
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (!cameraSwitch.isFirstPesonEnable)
        {
            // Rotation of de body player in Y axis
            playerBody.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
            
        }

        // Create Motion Vector Based on Keyboard Input
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Animations
        animator.SetFloat("VelX", moveHorizontal);
        animator.SetFloat("VelY", moveVertical);
        isMoving = move.magnitude > 0.1f;

        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isWalkingCrouched", isCrouching && isMoving);


        if (isMoving)
        {
            // Calculate direction of movement based on the player's rotation
            Vector3 moveDir = Quaternion.Euler(0, playerBody.eulerAngles.y, 0) * direction;
            controller.Move(moveDir * speedMove * Time.deltaTime);

            // Play footstep sound if you're on the move
            if (!steps.isPlaying)
            {
                steps.Play();
            }
        }
        else
        {
            // Pause footstep sound if not moving
            if (steps.isPlaying)
            {
                steps.Pause();
            }
        }

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Apply vertical movement (if crouching, speed changes)
        controller.Move(move * (isCrouching ? crouchSpeed : speedMove) * Time.deltaTime);
    }

    void HandleCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : standingHeight;

            float newCenterY = originalCenterY - (standingHeight - controller.height) / 2;
            controller.center = new Vector3(controller.center.x, newCenterY, controller.center.z);
        }
    }
    void AdjustCameraPosition()
    {
        if (cameraSwitch.isFirstPesonEnable)
        {
            cameraHolderFP.localPosition = isCrouching ?
                Vector3.Lerp(cameraHolderFP.localPosition, originalCameraLocalPositionFP + crouchCameraPositionOffsetFP, Time.deltaTime * 8f) :
                Vector3.Lerp(cameraHolderFP.localPosition, originalCameraLocalPositionFP + standingCameraPositionOffsetFP, Time.deltaTime * 8f);
        }
        else
        {
            cameraHolderTP.localPosition = isCrouching ?
                Vector3.Lerp(cameraHolderTP.localPosition, originalCameraLocalPositionTP + crouchCameraPositionOffsetTP, Time.deltaTime * 8f) :
                Vector3.Lerp(cameraHolderTP.localPosition, originalCameraLocalPositionTP + standingCameraPositionOffsetTP, Time.deltaTime * 8f);
        }
    }
    void HandleInput()
    {
        if (Time.timeScale==0) return;
        // Check if right mouse button is pressed and in third person view
        if (Input.GetButtonDown("Fire1") && isThirdPersonView)
        {
            // Check if knife is active
            if (isKnifeActive)
            {
                animator.SetBool("isKnifeAttacking", true); // Activar animación de ataque con cuchillo
                Debug.Log("Knife attack animation triggered");
            }
            else
            {
                animator.SetBool("isAttacking", true); // Si no hay cuchillo, activa otra animación de ataque
            }
        }

        // Reset attack state when button is released or not in third person view
        if (Input.GetButtonUp("Fire1") || !isThirdPersonView)
        {
            animator.SetBool("isKnifeAttacking", false); 
            animator.SetBool("isAttacking", false); 
        }
    }

    void HandleAimingAndCrouching()
    {
        isGunActive = gun.activeSelf;
        isKnifeActive = knife.activeSelf;  

        if (isThirdPersonView)
        {
            animator.SetBool("isAiming", isGunActive);
            animator.SetBool("isKnifeActive", isKnifeActive);

            if (isCrouching && isGunActive)
            {
                animator.SetBool("isCrouching", true);
                animator.SetBool("isAiming", true);
            }
            else if (isCrouching && isKnifeActive)
            {
                animator.SetBool("isCrouching", true);
                animator.SetBool("isKnifeActive", true);
            }
            else if (isKnifeActive)
            {
                animator.SetBool("isKnifeActive", true);
            }
            else
            {
                animator.SetBool("isCrouching", isCrouching);
                animator.SetBool("isAiming", isGunActive);
                animator.SetBool("isKnifeActive", isKnifeActive);
            }
        }
        else
        {
            animator.SetBool("isAiming", false);
            animator.SetBool("isKnifeActive", false);
            animator.SetBool("isCrouching", isCrouching);
        }
    }
    public void Attack()
    {
        // Lógica de ataque aquí
        Debug.Log("El personaje está atacando.");
    }
}