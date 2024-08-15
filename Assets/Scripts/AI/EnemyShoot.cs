using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    public GameObject enemyBullet;

    public Transform spawnBulletPoint;

    private Transform playerPosition;

    public float bulletVelocity = 100;
    // Start is called before the first frame update
    void Start()
    {
        playerPosition = FindObjectOfType<PlayerController>().transform;

        Invoke("ShootPlayer", 3);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShootPlayer()
    {
        Vector3 playerDirection = playerPosition.position - transform.position;

        GameObject newBullet = Instantiate(enemyBullet, spawnBulletPoint.transform.position, spawnBulletPoint.transform.rotation);

        newBullet.GetComponent<Rigidbody>().AddForce(playerDirection * bulletVelocity, ForceMode.Force);

        Invoke("ShootPlayer", 3);
    }
}
