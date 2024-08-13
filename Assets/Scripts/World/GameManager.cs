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

    public int health = 100;

    public int ammo = 10;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        ammoText.text = ammo.ToString();
        healthText.text = health.ToString();
    }
}
