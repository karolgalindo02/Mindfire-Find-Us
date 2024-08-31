using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50;
    private AI enemyAI; //AI script Reference
    [SerializeField] private float currentHealth;
    [SerializeField] private Image healthBar;

    void Start()
    {
        enemyAI = GetComponent<AI>();
        currentHealth = maxHealth;
        UpdateHealthBar();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        UpdateHealthBar();

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void UpdateHealthBar()
    {
        healthBar.fillAmount = currentHealth / maxHealth;
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
