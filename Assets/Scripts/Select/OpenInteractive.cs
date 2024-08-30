using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenInteractive : MonoBehaviour
{
    private Animator animator;
    private bool isOpen = false;

    [SerializeField] private AudioSource openInteractive;
    [SerializeField] private AudioSource closeInteractive;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public bool ToggleOpenClose()
    {
        if (animator != null)
        {
            isOpen = !isOpen;
            animator.SetBool("isOpen", isOpen);
            if (isOpen && openInteractive != null)
            {
                openInteractive.Play();
            }
            else if (!isOpen && closeInteractive != null)
            {
                closeInteractive.Play();
            }
        }
        return isOpen;
    }

    public bool IsOpen()
    {
        return isOpen;
    }
}