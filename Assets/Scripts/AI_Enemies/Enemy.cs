using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50;
    [SerializeField] private float currentHealth;
    [SerializeField] private Image healthBar;

    public void Start()
    {
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
        //Logic for the death of our enemy
        Destroy(gameObject);
    }
}
