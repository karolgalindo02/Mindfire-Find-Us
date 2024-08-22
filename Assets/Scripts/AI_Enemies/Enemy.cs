using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 50;

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        //Logic for the death of our enemy
        Destroy(gameObject);
    }
}
