using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using UnityEngine.UIElements;

public class PlayerControllerMF : MonoBehaviour
{
    [Header("Player settings")]
    [SerializeField] private float speedMove = 4.0f;
    [SerializeField] private float rotationSpeed = 80.0f;
    [SerializeField] private CharacterController controller;
    [SerializeField] private AudioSource steps;

    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed = 3.0f; // Crouching Speed
    [SerializeField] private float crouchHeight = 1.0f; // Height when crouching
    [SerializeField] private float standingHeight = 2.0f; // Standing height
    //[Header("Canvas")]

    //[Header("Animations")]

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
    [SerializeField] private bool isGrounded, isMoving, isGameOver;
    [SerializeField] private bool isCrouching = false;
    public bool isDead = false;




    void Start()
    {
        controller = GetComponent<CharacterController>();
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
    }

    void Update()
    {
        HandleMovement();

        HandleCrouch();
        
    }

    void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, shpereRadius, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            //Avoid mistakes, something like normalized de force that we apply for simulated gravity
            velocity.y = -2f;
        }

        // Character movement
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        isMoving = move.magnitude > 0.1f;

        if(isMoving && !steps.isPlaying)
        {
            steps.Play();
        }

        if(!isMoving && steps.isPlaying)
        {
            steps.Pause();
        }

        /*
         Template model for include Player animations
         if(currentAnimation != null)
        {
            currentAnimator.SetBool("isMoving", isMoving);
            currentAnimator.SetBool("isAttacking", isAttacking);
            currentAnimator.SetBool("isHitted", isHitted);
            currentAnimator.SetBool("isDead", isDead);
        }
         
         
         */
        // Apply gravity
        moveDirection.y -= 9.81f * Time.deltaTime;

        // Moving the character
        controller.Move(move * (isCrouching ? crouchSpeed : speedMove) * Time.deltaTime);

        // Rotating the Character with the Mouse

        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.1f)
        {
            //transform.rotation = Quaternion.Euler(new Vector3(0, mouseX * rotationSpeed * Time.deltaTime, 0));
            transform.Rotate(0, mouseX * rotationSpeed * Time.deltaTime, 0);
        }
        
        
        
        //transform.rotation = Quaternion.Euler(new Vector3(0, mouseX * rotationSpeed * Time.deltaTime, 0));

        //Force that we apply in "Y" Axis
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    void HandleCrouch()
    {
        // Logic for crouching
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = !isCrouching;
            controller.height = isCrouching ? crouchHeight : standingHeight;
        }
    }

    


}