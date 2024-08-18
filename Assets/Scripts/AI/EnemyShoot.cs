using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;

    public Transform spawnBulletPoint;

    public Transform playerPosition;

    public float bulletVelocity = 100;
    // Start is called before the first frame update 
    public float enemyShootRange = 10;

    public float enemyFieldOfView = 90;


    void Start()
    {
        //playerPosition = FindObjectOfType<PlayerController>().transform;

        Debug.Log(playerPosition.position);

        InvokeRepeating("ShootPlayer", 3, 2);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShootPlayer()
    {
       Vector3 directionToPlayer = playerPosition.position - transform.position;

       float distanceFromPlayer = directionToPlayer.magnitude;

       //Calculated the angle between player direction and enemy direction
       float angleToPlayer = Vector3.Angle(directionToPlayer, transform.forward);
       

        if (distanceFromPlayer < enemyShootRange && angleToPlayer < enemyFieldOfView/2)
        {
            //Shoot only if the player is in the field of view of the enemy
            Vector3 playerDirection = playerPosition.position - transform.position;

            GameObject newBullet = Instantiate(enemyBullet, spawnBulletPoint.transform.position, spawnBulletPoint.transform.rotation);

            newBullet.GetComponent<Rigidbody>().AddForce(directionToPlayer.normalized * bulletVelocity, ForceMode.Force);
        }
        


    }

 
}
