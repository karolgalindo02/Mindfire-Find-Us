using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    public float delay = 3;

    private float countdown;

    public float radius = 5;

    public float explosionForce = 70;

    private bool isExploded = false;

    public GameObject explosionEffect;

    private void Start()
    {
        //We set the time for it to explode.
        countdown = delay;
    }

    private void Update()
    {
        //We reduce the time for the grenade to explode.
        countdown -= Time.deltaTime;

        //We control that the grenade cannot explode more than once with the boolean isExploded
        if (countdown <= 0 && !isExploded)
        {
            Exploded();
            isExploded = true;
        }

    }

    private void Exploded()
    {
        //We created the particle effects explosion first version
        Instantiate(explosionEffect, transform.position, transform.rotation);
        //Detect all the elements around with colliders, we need to create an array and
        //use OverlapSphere() that asks us for 2 parameters: the current position of the grenade and the radius.
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var rangeObjects in colliders)
        {
            Rigidbody rb = rangeObjects.GetComponent<Rigidbody>();

            if (rb != null)
            {
                //In this case, it locates those elements with RB and adds force to them, simulating the explosion with physics.
                rb.AddExplosionForce(explosionForce * 10, transform.position, radius);
            }
        }
        Destroy(gameObject);
    }
}
