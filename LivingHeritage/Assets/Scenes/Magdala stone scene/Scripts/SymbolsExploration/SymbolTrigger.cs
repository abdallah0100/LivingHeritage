using UnityEngine;

public class SymbolTrigger : MonoBehaviour
{
    [Header("Info content")]
    public string title = "Title";

    [TextArea(5, 10)]
    public string body = "Body text here...";

    public Sprite photo;  // optional
    public SymbolType symbolType;

    [Header("Audio")]
    public SymbolAudio symbolAudio;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1️⃣ Play symbol audio
        if (symbolAudio != null)
            symbolAudio.PlaySymbolSound();

        // 2️⃣ Ask hotspot to show the panel using THIS data
        var hotspot = GetComponent<SymbolHotspot>();
        if (hotspot != null)
        {
            hotspot.ShowFrom(this);
        }
        else
        {
            Debug.LogWarning("SymbolTrigger: No SymbolHotspot found on this object.");
        }
    }
}
