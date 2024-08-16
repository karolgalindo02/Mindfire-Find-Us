using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemDrawer : MonoBehaviour
{
    [SerializeField] private bool drawerOpen = false;
    [SerializeField] private float drawerOpenPosition = 0.002f;
    [SerializeField] private float drawerClosePosition = 0f; 
    [SerializeField] private float smooth = 3f;
    private Vector3 initialPosition;

    public AudioClip openDrawer;
    public AudioClip closeDrawer;

    void Start()
    {
        initialPosition = transform.localPosition;
    }

    public void ChangeDrawerState()
    {
        drawerOpen = !drawerOpen;
    }

    void Update()
    {
        Vector3 targetPosition;
        if (drawerOpen)
        {
            targetPosition = new Vector3(initialPosition.x, initialPosition.y + drawerOpenPosition, initialPosition.z);
        }
        else
        {
            targetPosition = new Vector3(initialPosition.x, initialPosition.y + drawerClosePosition, initialPosition.z);
        }
        transform.localPosition = Vector3.Lerp(transform.localPosition, targetPosition, smooth * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("TriggerDrawer"))
        {
            AudioSource.PlayClipAtPoint(openDrawer, transform.position, 1);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("TriggerDrawer"))
        {
            AudioSource.PlayClipAtPoint(closeDrawer, transform.position, 1);
        }
    }
}