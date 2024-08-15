using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowGrenade : MonoBehaviour
{

    public float throwForce = 500;

    public GameObject grenadePrefab;

    void Update()
    { 
        //When press G key
        if (Input.GetKeyDown(KeyCode.G))
        {
            Throw();
        }
    }

    public void Throw()
    {
        GameObject newGrenade = Instantiate(grenadePrefab, transform.position, transform.rotation);

        //We assign to the RigidBody for applay the force for throw the grenade forward to us
        newGrenade.GetComponent<Rigidbody>().AddForce(transform.forward * throwForce);
    }

}
