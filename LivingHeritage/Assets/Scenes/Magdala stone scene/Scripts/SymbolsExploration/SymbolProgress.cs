using UnityEngine;
using System.Collections.Generic;

public enum SymbolType
{
    Menorah,
    Jar,
    Column,
    Rosette
}

public class SymbolProgress : MonoBehaviour
{
    public static SymbolProgress Instance { get; private set; }

    [Header("Stone that appears after all symbols")]
    [SerializeField] private GameObject stoneRebuildObject;

    private const int RequiredSymbolCount = 4;

    // Gameplay state
    private static HashSet<SymbolType> foundSymbols = new HashSet<SymbolType>();
    private static bool stoneShown = false;

    // ------------------ UNITY LIFECYCLE ------------------

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (stoneRebuildObject != null)
            stoneRebuildObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    // ------------------ SYMBOL REPORTING ------------------

    public static void ReportFound(SymbolType type)
    {
        if (Instance == null)
        {
            Debug.LogError("SymbolProgress.Instance is NULL");
            return;
        }

        // If stone was shown, this click starts a new run
        if (stoneShown)
        {
            ResetForNewRun();
        }

        // Ignore duplicates in the same run
        if (!foundSymbols.Add(type))
            return;

        Debug.Log($"Symbol found: {type} ({foundSymbols.Count}/{RequiredSymbolCount})");

        // Stone appears ONLY after all symbols
        if (foundSymbols.Count >= RequiredSymbolCount)
        {
            SessionVariables.completedStone = true;

            if (BadgeManager.Instance != null)
                BadgeManager.Instance.UnlockBadge(BadgeType.SymbolExplorer);

            ShowStone();
        }
    }

    // ------------------ RESET LOGIC ------------------

    private static void ResetForNewRun()
    {
        foundSymbols.Clear();
        stoneShown = false;

        if (Instance.stoneRebuildObject != null)
            Instance.stoneRebuildObject.SetActive(false);

        Debug.Log("SymbolProgress: New run initialized");
    }

    // ------------------ STONE CONTROL ------------------

    private static void ShowStone()
    {
        if (stoneShown)
            return;

        if (Instance == null || Instance.stoneRebuildObject == null)
        {
            Debug.LogError("Stone reference missing");
            return;
        }

        Instance.stoneRebuildObject.SetActive(true);
        stoneShown = true;

        Debug.Log("StoneRebuild object ACTIVATED");
    }

    // ------------------ API REQUIRED BY OTHER SCRIPTS ------------------

    // 🔥 FIXED: stone only shows if ALL symbols are found
    public static void TryShowStoneAfterPanelClose()
    {
        if (foundSymbols.Count >= RequiredSymbolCount)
            ShowStone();
    }

    // Used by HUDManager
    public static Dictionary<SymbolType, bool> GetSymbolStates()
    {
        var dict = new Dictionary<SymbolType, bool>();

        foreach (SymbolType type in System.Enum.GetValues(typeof(SymbolType)))
        {
            dict[type] = foundSymbols.Contains(type);
        }

        return dict;
    }
}
