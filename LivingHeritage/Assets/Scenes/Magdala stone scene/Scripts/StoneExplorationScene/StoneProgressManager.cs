using UnityEngine;
using UnityEngine.UI;

public class StoneProgressManager : MonoBehaviour
{
    public static StoneProgressManager Instance;

    public Image progressFill;
    public int totalSymbols = 4;

    int foundSymbols = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void SymbolFound()
    {
        foundSymbols++;
        progressFill.fillAmount =
            (float)foundSymbols / totalSymbols;
    }
}
