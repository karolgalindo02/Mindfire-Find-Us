using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;

    private float speedMove = 10f;
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
    private bool horiActivo;
    private bool vertiActivo;

    
    void Start()
    {
        
    }

    
    void Update()
    {
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
        if (Input.GetButtonDown("Horizontal") || Input.GetButtonDown("Vertical"))
    {
        if (!horiActivo && !vertiActivo)
        {
            pasos.Play();
        }

        if (Input.GetButtonDown("Horizontal"))
        {
            horiActivo = true;
        }
        if (Input.GetButtonDown("Vertical"))
        {
            vertiActivo = true;
        }
    }

    if (Input.GetButtonUp("Horizontal") || Input.GetButtonUp("Vertical"))
    {
        if (Input.GetButtonUp("Horizontal"))
        {
            horiActivo = false;
        }
        if (Input.GetButtonUp("Vertical"))
        {
            vertiActivo = false;
        }

        if (!horiActivo && !vertiActivo)
        {
            pasos.Pause();
        }
        
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
