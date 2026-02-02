using UnityEngine;
using TMPro;
using System.Collections;

public class SymbolPanel : MonoBehaviour
{
    public static SymbolPanel Instance;

    [Header("UI References")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI progressText;

    [Header("Settings")]
    public float visibleDuration = 2f;

    private void Awake()
    {
        Instance = this;
        gameObject.SetActive(false);
    }

    public void Show(string symbolName, int found, int total)
    {
        titleText.text = "" + symbolName;
        progressText.text = $"Found symbols: {found} / {total}";

        gameObject.SetActive(true);

        StopAllCoroutines();
        StartCoroutine(AutoHide());
    }

    private IEnumerator AutoHide()
    {
        yield return new WaitForSeconds(visibleDuration);
        gameObject.SetActive(false);
    }
}
