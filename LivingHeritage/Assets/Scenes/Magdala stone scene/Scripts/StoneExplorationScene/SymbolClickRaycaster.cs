using UnityEngine;
using UnityEngine.InputSystem;

public class SymbolClickRaycaster : MonoBehaviour
{
    private Camera cam;

    void Awake()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        // Mouse (Editor / Desktop)
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            TryHit(ray);
        }

        // Touch (Mobile)
        if (Touchscreen.current != null &&
            Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            Vector2 touchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Ray ray = cam.ScreenPointToRay(touchPos);
            TryHit(ray);
        }
    }

    void TryHit(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            ExplorationSymbolHotspot hotspot =
                hit.collider.GetComponent<ExplorationSymbolHotspot>();
            Debug.Log("Hit: " + hit.collider.name);


            if (hotspot != null)
            {
                hotspot.Activate();
            }

        }
    }
}
