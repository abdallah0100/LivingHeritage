using UnityEngine;

public class SymbolHotspot : MonoBehaviour
{
    [Header("Symbol model to hide/show")]
    [SerializeField] private GameObject symbolObject;

    [Header("Behaviour")]
    [Tooltip("If true, this symbol's panel will only appear once.")]
    [SerializeField] private bool showOnlyOnce = true;

    private static InfoPanelController sharedPanel;
    private static SymbolHotspot currentOpen;

    private bool isOpen = false;
    private bool hasShown = false;

    public void ShowFrom(SymbolTrigger data)
    {
        // 🔍 Find the shared panel if needed
        if (sharedPanel == null)
        {
            sharedPanel = FindObjectOfType<InfoPanelController>(true);

            if (sharedPanel == null)
            {
                Debug.LogError("SymbolHotspot: No InfoPanelController found!");
                return;
            }
        }

        // ✅ ALWAYS update current symbol audio
        sharedPanel.currentSymbolAudio = data.symbolAudio;

        // Show-once logic
        if (showOnlyOnce && hasShown)
            return;

        hasShown = true;

        // Report progress
        SymbolProgress.ReportFound(data.symbolType);

        // Close previously open symbol
        if (currentOpen != null && currentOpen != this)
            currentOpen.ForceClose();

        currentOpen = this;
        isOpen = true;

        // Show panel
        sharedPanel.Show(
            data.title,
            data.body,
            data.photo,
            null,
            OnPanelClosed
        );
    }

    private void OnPanelClosed()
    {
        isOpen = false;

        if (currentOpen == this)
            currentOpen = null;

        // Progress & badges
        SymbolProgress.TryShowStoneAfterPanelClose();

        if (BadgeManager.Instance != null)
            BadgeManager.Instance.TryShowPendingPopups();
    }

    public void ForceClose()
    {
        if (!isOpen || sharedPanel == null) return;
        sharedPanel.Close();
    }

    private void SetSymbolVisible(bool visible)
    {
        if (symbolObject == null) return;

        foreach (var r in symbolObject.GetComponentsInChildren<Renderer>(true))
            r.enabled = visible;

        foreach (var c in symbolObject.GetComponentsInChildren<Collider>(true))
            c.enabled = visible;
    }
}
