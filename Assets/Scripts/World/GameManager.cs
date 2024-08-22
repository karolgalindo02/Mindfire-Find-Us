using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager Instance { get; private set; }

    //Canvas UI Ammo
    public TextMeshProUGUI ammoText;
    public TextMeshProUGUI healthText;

    [SerializeField] public int health = 100;

    [SerializeField] private int maxHealth = 100;

    [SerializeField] public int ammo = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ammoText.text = ammo.ToString();
        healthText.text = health.ToString();
    }

    public void LoseHealth(int healthToReduce)
    {
        health -= healthToReduce;
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            //Logic for gameOver
            Debug.Log("You have died");
        }
    }
    //Control of health limit 100
    public void AddHealth(int healthToIncrease)
    {
        if(this.health + healthToIncrease >= maxHealth)
        {
            this.health = 100;
        }
        else
        {
            this.health += healthToIncrease;
        }
    }
}
