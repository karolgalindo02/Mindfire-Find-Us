using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player; // Player reference
    public Vector3 offset; // Camera Movement Relative to the Player
    private float xRotation = 0;
    public float mouseSensitivity = 80f;
    //For check that the game is initialized and the gun start in the center of the screen
    private bool isInitialized = false;

    private bool isMouseActive = false; // Controlar si el mouse está activo




    void Start()
    {
        //Block de mouse within the limits of the screen
        Cursor.lockState = CursorLockMode.Locked;

        // Temporarily disable mouse input
        StartCoroutine(EnableMouseInput());

        Cursor.visible = false;
        xRotation = 0;

        transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);

        

    }

    void LateUpdate()
    {
        if (Cursor.lockState == CursorLockMode.Locked && isInitialized)
        {
            //Follow player
            transform.position = player.position + offset;

            //Vertical camera rotation with the mouse
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;


            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90f, 90f); // Limit vertical rotation

            //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            //transform.position = player.position + offset;

            //transform.rotation = Quaternion.Euler(0, player.eulerAngles.y, 0);

            transform.rotation = Quaternion.Euler(xRotation, player.eulerAngles.y, 0f);
        }
            
    
    }

    IEnumerator EnableMouseInput()
    {
        // wait a miliseconds for the game initialized in the correct position
        yield return new WaitForSeconds(0.3f);

        while (!isMouseActive)
        {
            if(Input.mousePosition.y >= 0 && Input.mousePosition.y <= Screen.height)
            {
                isMouseActive = true;
            }
            yield return null;
        }
        isInitialized = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
