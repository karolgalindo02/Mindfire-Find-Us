using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    //Spwan point
    public Transform spawnPoint;

    public GameObject bullet;

    public float shotForce = 1500f;

    //Interval for shot
    public float shotRate = 0.5f;

    //Counter for shot as a gun
    private float shotRateTime = 0;

    void Start()
    {
        
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (Time.time > shotRateTime)
            {
                GameObject newBullet;

                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);


                //Add force forward for impulse de bullet
                newBullet.GetComponent<Rigidbody>().AddForce(spawnPoint.forward * shotForce);

                //Controller of the time for shot, if you press play, the Time.time is mayor than shotRateTime, because Time.time could be 1f, y shotRateTime is 0
                //Then, when you shot the first time, shotRateTime, take the time and add 0.5f, so, the shotRateTime allow to shot every 0.5f seconds, no all the time
                //Like a machine gun, it´s just a gun
                shotRateTime = Time.time + shotRate;

                //Destroy our bullet
                Destroy(newBullet, 2f);
            }
            

        }
    }
}
