using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;



public class SymbolDiscoveryManager : MonoBehaviour
{
    public static SymbolDiscoveryManager Instance;

    public int totalSymbols = 3;
    private HashSet<string> discoveredSymbols = new HashSet<string>();

    [Header("UI")]
    public Image progressFill;
    public GameObject continueQuizButton;

    [Header("Animation")]
    public float fillAnimationDuration = 0.4f;

    Coroutine fillRoutine;

    private void Awake()
    {
        Instance = this;
    }

    public bool DiscoverSymbol(string symbolName)
    {
        if (discoveredSymbols.Contains(symbolName))
            return false;

        discoveredSymbols.Add(symbolName);

        AnimateProgress();   // 🔥 smooth animation

        if (discoveredSymbols.Count == totalSymbols)
            OnAllSymbolsDiscovered();

        return true;
    }

    private void AnimateProgress()
    {
        if (progressFill == null)
            return;

        float target =
            (float)discoveredSymbols.Count / totalSymbols;

        if (fillRoutine != null)
            StopCoroutine(fillRoutine);

        fillRoutine = StartCoroutine(
            SmoothFill(progressFill.fillAmount, target)
        );
    }

    IEnumerator SmoothFill(float start, float target)
    {
        float time = 0f;

        while (time < fillAnimationDuration)
        {
            time += Time.deltaTime;
            float t = time / fillAnimationDuration;

            // Smooth easing
            progressFill.fillAmount =
                Mathf.Lerp(start, target, Mathf.SmoothStep(0f, 1f, t));

            yield return null;
        }

        progressFill.fillAmount = target;
    }

    private void OnAllSymbolsDiscovered()
    {
        continueQuizButton.SetActive(true);
    }

    public int Discovered => discoveredSymbols.Count;
    public int Total => totalSymbols;


}
