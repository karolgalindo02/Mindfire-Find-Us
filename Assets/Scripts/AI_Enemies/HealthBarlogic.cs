using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarlogic : MonoBehaviour
{
    [SerializeField] public int maxHealth;
    [SerializeField] public int currentHealth;
    [SerializeField] public Image healthBarImage;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        CheckHealth();
        if(currentHealth <= 0)
        {

        }
    }

    public void CheckHealth()
    {
        healthBarImage.fillAmount = currentHealth / maxHealth;
    }
}
