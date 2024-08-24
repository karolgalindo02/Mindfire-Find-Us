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
    [SerializeField] private Transform cameraHolder;
    [SerializeField] private float rotationSpeed = 80f;

    [Header("Activate rotation 3P")]
    //for limit the rotation between cameras
    [SerializeField] CameraSwitch cameraSwitch;
   


    [Header("Crouch Settings")]
    [SerializeField] private float crouchSpeed = 3.0f; // Crouching Speed
    [SerializeField] private float crouchHeight = 1.0f; // Height when crouching
    [SerializeField] private float standingHeight = 1.7f; // Standing height
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
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        //transform.localRotation = Quaternion.Euler(0, 0, 0);
        animator =GetComponent<Animator>();
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
            // Asegurar que el jugador esté bien alineado con el suelo
            velocity.y = -2f;
        }

        // Obtener entrada del mouse
        float mouseX = Input.GetAxis("Mouse X");

        float mouseY = Input.GetAxis("Mouse Y");

        // Obtener entrada del teclado
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (!cameraSwitch.isFirstPesonEnable)
        {
            Debug.Log("I´m with on 3camera mode");
            // Rotation of de body player in Y axis
            playerBody.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
            //playerBody.Rotate(mouseX * rotationSpeed * Time.deltaTime * Vector3.up);
        }

        // Crear vector de movimiento en base a la entrada del teclado
        Vector3 direction = new Vector3(moveHorizontal, 0, moveVertical).normalized;
        Vector3 move = transform.right * moveHorizontal + transform.forward * moveVertical;

        // Animaciones
        animator.SetFloat("VelX", moveHorizontal);
        animator.SetFloat("VelY", moveVertical);
        isMoving = move.magnitude > 0.1f;

        if (isMoving)
        {
            // Calcular dirección del movimiento en base a la rotación del jugador
            Vector3 moveDir = Quaternion.Euler(0, playerBody.eulerAngles.y, 0) * direction;
            controller.Move(moveDir * speedMove * Time.deltaTime);

            // Reproducir sonido de pasos si está en movimiento
            if (!steps.isPlaying)
            {
                steps.Play();
            }
        }
        else
        {
            // Pausar sonido de pasos si no se está moviendo
            if (steps.isPlaying)
            {
                steps.Pause();
            }
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Aplicar movimiento vertical (si se está agachando, la velocidad cambia)
        controller.Move(move * (isCrouching ? crouchSpeed : speedMove) * Time.deltaTime);
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