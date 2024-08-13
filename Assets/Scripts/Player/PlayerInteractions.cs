using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ammunition"))
        {
            //Here we add the number of variable ammo from the script AmunitionBox to the variable in the instance of the GamaManager
            GameManager.Instance.ammo += other.gameObject.GetComponent<AmmunitionBox>().ammo;
            //And then the box disappears
            Destroy(other.gameObject);
        }
    }
}
