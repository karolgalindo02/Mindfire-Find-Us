using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    private Quaternion startRotation;

    public float swayAmount = 8;
   
    void Start()
    {
        //Obtain the initial rotation of the gun
        startRotation = transform.localRotation;
    }

    void Update()
    {
        Sway();
    }
    private void Sway()
    {
        //Variables to saved the move of the mouse in the Axis X or Y
        float mouseX = Input.GetAxis("Mouse X");

        float mouseY = Input.GetAxis("Mouse Y");

        //The angles that we need for the rotation animated
        Quaternion xAngle = Quaternion.AngleAxis(mouseX * -1.25f, Vector3.up);

        Quaternion yAngle = Quaternion.AngleAxis(mouseY * -1.25f, Vector3.right);

        //The target or the goal that we want to achive
        Quaternion targetRotation = startRotation * xAngle * yAngle;


        //Final rotation, the last operation between Time.deltaTime * swayAmount, is for apply de soft movement, when the rotation happens
        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * swayAmount);



    }
}
