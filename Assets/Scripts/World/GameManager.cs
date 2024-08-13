using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Singleton pattern
    public static GameManager Instance { get; private set; }

    public int ammo = 10;

    private void Awake()
    {
        Instance = this;
    }
}
