using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    [SerializeField] private float speed;
    private Transform player;
    private Rigidbody rb;
    [SerializeField] private LayerMask groundLayer;



    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Center").transform;   
        rb = GetComponent<Rigidbody>();

        LaunchProjectile();
    }

    private void OnCollisionEnter(Collision collision)
    {
        //When the object is initialize and enter in a collision with other object is destroyed
        if (collision.gameObject.layer == groundLayer || collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        
    }

    private void LaunchProjectile()
    {
        Vector3 directionPlayer = (player.position - transform.position).normalized;
        rb.AddForce(directionPlayer * speed);
        
    }

   

}
