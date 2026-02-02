using System.Collections.Generic;
using UnityEngine;

public class BadgeManager : MonoBehaviour
{
    public static BadgeManager Instance;

    [Header("Badge Icons")]
    public Sprite symbolExplorerIcon;

    private HashSet<BadgeType> unlockedBadges = new HashSet<BadgeType>();

    // 🔒 Popup should wait until InfoPanel closes
    private bool symbolExplorerPendingPopup = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // optional but recommended
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UnlockBadge(BadgeType badge)
    {
        if (unlockedBadges.Contains(badge)) return;

        unlockedBadges.Add(badge);
        Debug.Log($"🏅 Badge unlocked: {badge}");

        // Do NOT show popup yet
        if (badge == BadgeType.SymbolExplorer)
            symbolExplorerPendingPopup = true;
    }

    // 🔔 Called AFTER the InfoPanel closes
    public void TryShowPendingPopups()
    {
        if (!symbolExplorerPendingPopup) return;
        if (BadgePopupController.Instance == null) return;

        BadgePopupController.Instance.Show(
            "",
            symbolExplorerIcon
        );

        symbolExplorerPendingPopup = false;
    }

    public bool HasBadge(BadgeType badge)
    {
        return unlockedBadges.Contains(badge);
    }
}
