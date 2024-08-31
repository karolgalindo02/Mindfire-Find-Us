using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarHealthEnemyFollow : MonoBehaviour
{

    void Update()
    {
        transform.forward = CameraSwitch.activeCamera.transform.forward;
    }
}
