using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    //Spwan point
    public Transform spawnPoint;

    public GameObject bullet;

    public float shotForce = 1500f;

    //Interval for shot
    public float shotRate = 0.5f;

    //Counter for shot as a gun
    private float shotRateTime = 0;

    public Camera mainCamera;
    public AudioSource shotSound;
    public AudioSource emptySound;
    void Start()
    {
         if (shotSound == null)
        {
            shotSound = GetComponent<AudioSource>();
        }
    }

    
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            //We check with the && symbol if we had ammo
            if (Time.time > shotRateTime && GameManager.Instance.ammo > 0)
            {
                //When we shoot the ammo needs to be reduced
                GameManager.Instance.ammo--;

                GameObject newBullet;

                newBullet = Instantiate(bullet, spawnPoint.position, spawnPoint.rotation);
                shotSound.Play();

                //Calculate the direction based on the crosshair
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

                RaycastHit hit;

                Vector3 targetPoint;

                if(Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                    Debug.Log($"TargetPoint: {targetPoint}");
                }
                else
                {
                    targetPoint = ray.GetPoint(1000); //A distant poin in the direction of the raycast
                }

                Vector3 direction = targetPoint - spawnPoint.position;

                //Calculate the direction based on the crosshair
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

                RaycastHit hit;

                Vector3 targetPoint;

                if(Physics.Raycast(ray, out hit))
                {
                    targetPoint = hit.point;
                    Debug.Log($"TargetPoint: {targetPoint}");
                }
                else
                {
                    targetPoint = ray.GetPoint(1000); //A distant poin in the direction of the raycast
                }

                Vector3 direction = targetPoint - spawnPoint.position;


                //Add force forward for impulse de bullet
                //We replace here spwanPoint.forward by direction.normalized
                newBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shotForce);

                //Controller of the time for shot, if you press play, the Time.time is mayor than shotRateTime, because Time.time could be 1f, y shotRateTime is 0
                //Then, when you shot the first time, shotRateTime, take the time and add 0.5f, so, the shotRateTime allow to shot every 0.5f seconds, no all the time
                //Like a machine gun, it�s just a gun
                shotRateTime = Time.time + shotRate;

                //Destroy our bullet
                Destroy(newBullet, 2f);
            }
            else{
                    // Play empty sound when no ammo
                if (emptySound != null)
                {
                    emptySound.Play();
                }
                
            } 
            
        }
    }
}
