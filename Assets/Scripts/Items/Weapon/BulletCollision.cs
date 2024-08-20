using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCollision : MonoBehaviour
{
    [SerializeField] private int bulletDamage = 15;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            //Destroy(collision.gameObject);
            Debug.Log("You hit your enemy with a Gun");
            collision.gameObject.GetComponent<Enemy>().TakeDamage(bulletDamage);
        }
    }
}
