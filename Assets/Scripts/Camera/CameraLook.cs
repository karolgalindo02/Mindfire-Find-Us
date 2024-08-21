using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    //player body
    [SerializeField] private Transform playerBody;

    float xRotation = 0;

    //Mouse sensivity
    public float mouseSensitivity = 80f;

    //For check that the game is initialized and the gun start in the center of the screen
    private bool isInitialized = false;




    void Start()
    {
        //Block de mouse within the limits of the screen
        Cursor.lockState = CursorLockMode.Locked;

        xRotation = 0;

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        // Desactiva temporalmente la entrada del mouse
        StartCoroutine(EnableMouseInput());
    }


    void Update()
    {
        if (Cursor.lockState == CursorLockMode.Locked && isInitialized)
        {


            //variables to save the direction of the mouse in the axis "X" and "Y"
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;

            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

            //Setting for the rotation in "X" axis with camera
            xRotation -= mouseY;
            //limit of bound rotation in "X" Axis with camera
            xRotation = Mathf.Clamp(xRotation, -90f, 90f);

            //Convert de rotation of the camera in the rotation of the mouse with the player body
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            //Body player rotate in the Y axis with de movement of the mouse in the horizontal Axis
            playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    IEnumerator EnableMouseInput()
    {
        // wait a miliseconds for the game initialized in the correct position
        yield return new WaitForSeconds(0.3f);
        isInitialized = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

}
