using UnityEngine;

public class ExplorationSymbolHotspot : MonoBehaviour
{
    public string symbolTitle;
    public GameObject symbol3DPrefab;

    private bool discovered = false;

    public void Activate()
    {
        bool newlyDiscovered =
            SymbolDiscoveryManager.Instance.DiscoverSymbol(symbolTitle);

        if (!newlyDiscovered)
            return;

        SymbolPanel.Instance.Show(
            symbolTitle,
            SymbolDiscoveryManager.Instance.Discovered,
            SymbolDiscoveryManager.Instance.Total
        );

        if (symbol3DPrefab != null)
        {
            Symbol3DViewer.Instance.ShowSymbol(symbol3DPrefab);
        }
    }

}
