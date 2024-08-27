using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NavKeypad
{
    public class KeypadInteractionFPV : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private void Awake()
        {
            if (cam == null)
            {
                cam = Camera.main;
                if (cam == null)
                {
                    Debug.LogError("No camera assigned and no main camera found in the scene.");
                }
            }
        }

        private void Update()
        {
            if (cam == null) return;

            var ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Input.GetMouseButtonDown(0))
            {
                if (Physics.Raycast(ray, out var hit))
                {
                    if (hit.collider.TryGetComponent(out KeypadButton keypadButton))
                    {
                        keypadButton.PressButton();
                    }
                }
            }
        }
    }
}