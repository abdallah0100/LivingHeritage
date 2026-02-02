using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class SliderColorGradient : MonoBehaviour
{
    [Header("References")]
    public Image fillImage; // assign the Fill image here in the Inspector
    public TextMeshProUGUI expLevel;

    [Header("Colors")]
    public Color startColor = Color.yellow;  // Start (0%)
    public Color midColor = Color.green;     // Mid (50%)
    public Color endColor = Color.blue;      // End (100%)

    private Slider slider;

    void Awake()
    {
        slider = GetComponent<Slider>();
    }

    void Update()
    {
        if (fillImage == null) return;

        float t = slider.normalizedValue; // value between 0–1

        // Two-step gradient: Yellow -> Green -> Blue
        if (t < 0.5f)
        {
            // Interpolate between Yellow (0) and Green (0.5)
            fillImage.color = Color.Lerp(startColor, midColor, t * 2f);
        }
        else
        {
            // Interpolate between Green (0.5) and Blue (1)
            fillImage.color = Color.Lerp(midColor, endColor, (t - 0.5f) * 2f);
        }
        updateExpText(slider.value);
    }

    private void updateExpText(float level) {
        string levelText = "";
        if (level == 0)
            levelText = "New";
        else if (level > 0 && level < 1)
            levelText = "Beginner";
        else if (level > 1 && level < 3.4)
            levelText = "Experienced";
        else if (level > 3.4 && level < 4.2)
            levelText = "Experienced+";
        else
            levelText = "Advanced";
        expLevel.SetText(levelText);
        expLevel.color = fillImage.color;
    }
}
