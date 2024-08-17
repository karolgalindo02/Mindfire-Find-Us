using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    [SerializeField] private float speedMove = 5f;
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

    //For Player jump
    public float jumpHeight = 3;

    public AudioSource pasos;
    
    void Start()
    {
        
    }

    
    void Update()
    {
        if (Time.timeScale==0) return;
        //Check is the player is grounded
        isGrounded = Physics.CheckSphere(groundCheck.position, shpereRadius, groundMask);

        if(isGrounded && velocity.y < 0)
        {
            //Avoid mistakes, something like normalized de force that we apply for simulated gravity
            velocity.y = -2f;
        }

        //We save the values of the keyboard in variables moveX or moveZ
        float moveX = Input.GetAxis("Horizontal");

        float moveZ = Input.GetAxis("Vertical");
        
        //Sonido de pasos
         if (IsMoving() && !pasos.isPlaying)
        {
            pasos.Play();
        }

        // Detén el sonido de pasos si el jugador deja de moverse
        if (!IsMoving() && pasos.isPlaying)
        {
            pasos.Pause();
        }


        //Vector that we need for set the position of the player when the player press a key
        Vector3 move = transform.right * moveX + transform.forward * moveZ;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        //Player movement
        characterController.Move(move * speedMove * Time.deltaTime);

        //Force that we apply in "Y" Axis
        velocity.y += gravity * Time.deltaTime;

        characterController.Move(velocity * Time.deltaTime);
    }
        public bool IsMoving()
    {
        return Input.GetButton("Horizontal") || Input.GetButton("Vertical");
    }
}
