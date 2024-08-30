using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float health = 50;

    private AI enemyAI; //AI script Reference

    void Start()
    {
        enemyAI = GetComponent<AI>(); 
    }

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
        //Desativate the IA for the enemy dont continue walking or atacking
        if (enemyAI != null)
        {
            enemyAI.isDead = true;
            enemyAI.Die();
        }
        //Star the corutine to wait before destroy the object
        StartCoroutine(WaitForDeathAnimation());
    }

    private IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3f); 
        Destroy(gameObject);
    }
}
