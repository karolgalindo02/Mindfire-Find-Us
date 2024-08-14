using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("Scripts/Weapon/GunShoot")]
public class GunShoot : MonoBehaviour
{
    [Header("Prefab Refrences")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;

    [Header("Location Refrences")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [Tooltip("Specify time to destory the casing object")] [SerializeField] private float destroyTimer = 2f;
    [Tooltip("Bullet Speed")] [SerializeField] private float shotPower = 1500f;
    [Tooltip("Casing Ejection Speed")] [SerializeField] private float ejectPower = 150f;

    //Interval for shot
    public float shotRate = 0.5f;

    //Counter for shot as a gun
    private float shotRateTime = 0;

    public Camera mainCamera;
    public AudioSource shotSound;
    public AudioSource emptySound;

    void Start()
    {
        if (barrelLocation == null){
            barrelLocation = transform;
        }
        if (gunAnimator == null){
            gunAnimator = GetComponentInChildren<Animator>();
        }
        if (shotSound == null){
            shotSound = GetComponent<AudioSource>();
        }
    }

    void Update()
    {
        //If you want a different input, change it here
        if (Input.GetButtonDown("Fire1"))
        {
            if(GameManager.Instance.ammo > 0)
            {
                shotSound.Play();

                GameManager.Instance.ammo--;
                //Calls animation on the gun that has the relevant animation events that will fire
                gunAnimator.SetTrigger("Fire");
                if (GameManager.Instance.ammo >= 0)
                {
                    Shoot();
                }
            } else{       
                // Play empty sound when no ammo
                if (GameManager.Instance.ammo==0){
                
                    emptySound.Play();
                }
            }   

            
        }
    }


    //This function creates the bullet behavior
    void Shoot()
    {
        if (muzzleFlashPrefab)
        {
            //Create the muzzle flash
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);

            //Destroy the muzzle flash effect
            Destroy(tempFlash, destroyTimer);
        }

        //cancels if there's no bullet prefeb
        if (!bulletPrefab)
        { return; }
        if(Time.time > shotRateTime && GameManager.Instance.ammo > 0)
        {
            
            // Create a bullet and add force on it in direction of the barrel
            //Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation).GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);
            GameObject newBullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);

            //Calculate the direction based on the crosshair
            Ray ray = mainCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));

            RaycastHit hit;

            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
                Debug.Log($"TargetPoint: {targetPoint}");
            }
            else
            {
                targetPoint = ray.GetPoint(1000); //A distant poin in the direction of the raycast
            }

            Vector3 direction = targetPoint - barrelLocation.position;

            newBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * shotPower);

            shotRateTime = Time.time + shotRate;
            Destroy(newBullet, 2f);

        }            

    }

    //This function creates a casing at the ejection slot
    void CasingRelease()
    {
        //Cancels function if ejection slot hasn't been set or there's no casing
        if (!casingExitLocation || !casingPrefab)
        { return; }

        //Create the casing
        GameObject tempCasing;
        tempCasing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        //Add force on casing to push it out
        tempCasing.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(ejectPower * 0.7f, ejectPower), (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f);
        //Add torque to make casing spin in random direction
        tempCasing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(100f, 1000f)), ForceMode.Impulse);

        //Destroy casing after X seconds
        Destroy(tempCasing, destroyTimer);
    }

}
