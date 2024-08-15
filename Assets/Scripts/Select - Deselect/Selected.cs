using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selected : MonoBehaviour
{
    LayerMask mask;
    [SerializeField] private float distance = 1.5f;
    void Start()
    {
        mask = LayerMask.GetMask("RaycastDetect");
    }
    void Update()
    {
        //Raycast(origin, direction, hitInfo, distance, layerMask)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, distance, mask))
        {
            if (hit.collider.tag == "Ammunition")
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    hit.collider.gameObject.GetComponent<InteractiveItem>().OnInteract();
                }
            }
        }
    }
}
