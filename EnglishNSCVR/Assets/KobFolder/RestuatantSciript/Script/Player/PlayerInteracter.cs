using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class PlayerInteracter : MonoBehaviour
{
    [Header("Settings")]
    public float interactRange = 5f;
    public LayerMask uiLayer; // เลือก Layer "UI" หรือ Layer ที่ Canvas อยู่

    [Header("Crosshair")]
    public Image crosshairImage;
    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    public Camera cam;


    void Update()
    {
        Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange))
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                crosshairImage.color = hoverColor;

                if (Input.GetKeyDown(KeyCode.E))
                {
                    interactable.Interact();
                }
            }
            else
            {
                crosshairImage.color = normalColor;
            }
        }
        else
        {
            crosshairImage.color = normalColor;
        }
    }


}