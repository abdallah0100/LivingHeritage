using UnityEngine;
using UnityEngine.EventSystems;

public class ToolActionRaycaster : MonoBehaviour
{
    public Camera cam;
    public LayerMask mask = ~0;          // set to Everything for now
    public float maxDistance = 100f;

    void Awake()
    {
        if (cam == null) cam = Camera.main;
    }

    void Update()
    {
        // --- Mobile touch ---
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);

            // If the finger is over UI, don't interact with world (tools/buttons/etc.)
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject(t.fingerId))
                return;

            // Tap for shovel (rubble)
            if (t.phase == TouchPhase.Began)
                HandleTap(t.position);

            // Swipe/hold for brush (sand overlay)
            if (t.phase == TouchPhase.Moved || t.phase == TouchPhase.Stationary)
                HandleBrushDrag(t.position);

            return;
        }

#if UNITY_EDITOR
    // --- Editor mouse testing ---
    if (Input.GetMouseButtonDown(0))
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleTap(Input.mousePosition);
    }

    if (Input.GetMouseButton(0))
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleBrushDrag(Input.mousePosition);
    }
#endif
    }


    void HandleTap(Vector2 screenPos)
    {
        if (ToolSelectionManager.Instance == null) return;

        // Shovel cleans rubble
        if (!ToolSelectionManager.Instance.IsSelected(ToolType.Shovel))
            return;

        if (cam == null) return;

        Ray ray = cam.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask, QueryTriggerInteraction.Collide))
        {
            var cleanable = hit.collider.GetComponentInParent<RubbleCleanable>();
            if (cleanable != null)
                cleanable.Clean();
        }
    }

    void HandleBrushDrag(Vector2 screenPos)
    {
        // Ignore swipe if finger is over UI (buttons/tools/etc.)
        // For mobile touch, pass the fingerId from Update (recommended)
        // If you can't, this still helps for mouse/editor.
        if (EventSystem.current != null)
        {
#if UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("[Brush] Ignored: pointer over UI (editor/mouse)");
            return;
        }
#endif
        }

        if (ToolSelectionManager.Instance == null)
        {
            Debug.Log("[Brush] ToolSelectionManager is NULL");
            return;
        }

        if (!ToolSelectionManager.Instance.IsSelected(ToolType.Brush))
        {
            Debug.Log("[Brush] Brush NOT selected");
            return;
        }

        if (cam == null)
        {
            Debug.Log("[Brush] Camera is NULL");
            return;
        }

        Ray ray = cam.ScreenPointToRay(screenPos);

        if (Physics.Raycast(ray, out RaycastHit hit, maxDistance, mask, QueryTriggerInteraction.Collide))
        {
            Debug.Log($"[Brush] Ray HIT: {hit.collider.name} (layer={LayerMask.LayerToName(hit.collider.gameObject.layer)})");

            var sand = hit.collider.GetComponentInParent<SandOverlayBrushable>();
            if (sand != null)
            {
                Debug.Log("[Brush] SandOverlayBrushable found -> brushing");
                sand.Brush(Time.deltaTime);
            }
            else
            {
                Debug.Log("[Brush] Hit object is NOT the sand overlay (no SandOverlayBrushable in parents)");
            }
        }
        else
        {
            Debug.Log("[Brush] Raycast MISSED (no collider hit)");
        }
    }
}
