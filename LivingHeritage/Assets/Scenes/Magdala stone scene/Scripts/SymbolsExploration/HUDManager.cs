using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;

public class HUDManager : MonoBehaviour
{
    [Header("Panels")]
    public CanvasGroup instructionsPanel;
    public CanvasGroup progressPanel;

    [Header("Progress UI")]
    public Transform progressContentParent;
    public GameObject progressRowPrefab;

    private List<GameObject> createdRows = new List<GameObject>();

    private void Start()
    {
        ShowInstructions();
    }

    // ------------------ Show Progress Panel ------------------

    public void ShowProgress()
    {
        SetPanelVisible(progressPanel, true);
        RefreshProgressUI();
    }

    public void HideProgress()
    {
        SetPanelVisible(progressPanel, false);
    }

    // ------------------ Refresh UI ------------------

    private void RefreshProgressUI()
    {
        foreach (GameObject row in createdRows)
            Destroy(row);

        createdRows.Clear();

        var states = SymbolProgress.GetSymbolStates();

        foreach (var item in states)
        {
            GameObject rowObj = Instantiate(progressRowPrefab, progressContentParent);
            createdRows.Add(rowObj);

            TextMeshProUGUI label = rowObj.transform.Find("Label").GetComponent<TextMeshProUGUI>();
            Image iconSymbol = rowObj.transform.Find("Icon").GetComponent<Image>();

            Transform status = rowObj.transform.Find("IconStatus");
            GameObject iconCheck = status.Find("IconCheck").gameObject;
            GameObject iconX = status.Find("IconX").gameObject;

            label.text = item.Key.ToString();

            bool isFound = item.Value;

            iconCheck.SetActive(isFound);
            iconX.SetActive(!isFound);

            iconSymbol.sprite = GetSymbolSprite(item.Key);
        }
    }

    // ------------------ Symbol Icons ------------------

    public Sprite iconMenorah;
    public Sprite iconJar;
    public Sprite iconColumn;
    public Sprite iconRosette;

    private Sprite GetSymbolSprite(SymbolType type)
    {
        return type switch
        {
            SymbolType.Menorah => iconMenorah,
            SymbolType.Jar => iconJar,
            SymbolType.Column => iconColumn,
            SymbolType.Rosette => iconRosette,
            _ => null
        };
    }

    // ------------------ Helper ------------------

    private void SetPanelVisible(CanvasGroup panel, bool visible)
    {
        panel.alpha = visible ? 1 : 0;
        panel.blocksRaycasts = visible;
        panel.interactable = visible;
    }

    public void ShowInstructions() => SetPanelVisible(instructionsPanel, true);
    public void HideInstructions() => SetPanelVisible(instructionsPanel, false);
}
