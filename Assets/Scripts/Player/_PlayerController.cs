using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class _PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 100.0f;

    public float crouchSpeed = 3.0f; // Crouching Speed
    public float crouchHeight = 1.0f; // Height when crouching
    public float standingHeight = 2.0f; // Standing height

    //Because we dont use RigidBody
    private float gravity = -9.81f;

    //This is the position of the gameObject empty for check is our player touched the floor
    [SerializeField] private Transform groundCheck;

    //Variable that is configured with the empty object that we set under the Player to check whether or not it is touching the ground.
    private bool isGrounded;

    public float shpereRadius = 0.3f;
    //Mask that we created for our scene to defined what is or what is not floor
    public LayerMask groundMask;

    //Variable to saved the force that we apply in the Axis "Y" for simulated the gravity
    private Vector3 velocity;


    private bool isCrouching = false;


    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;



    void Start()
    {
        controller = GetComponent<CharacterController>();
        //transform.rotation = Quaternion.Euler(0, 0, 0);
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

        // Apply gravity
        moveDirection.y -= 9.81f * Time.deltaTime;

        // Moving the character
        controller.Move(move * (isCrouching ? crouchSpeed : speed) * Time.deltaTime);

        // Rotating the Character with the Mouse

        
        float mouseX = Input.GetAxis("Mouse X");
        if (Mathf.Abs(mouseX) > 0.1f)
        {
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

    public bool IsMoving()
    {
        // Considera movimiento si la entrada horizontal o vertical es mayor que 0.1 (puedes ajustar el valor si es necesario)
        return Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f || Mathf.Abs(Input.GetAxis("Vertical")) > 0.1f;
    }


}