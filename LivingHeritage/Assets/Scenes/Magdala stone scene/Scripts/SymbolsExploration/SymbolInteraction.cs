using UnityEngine;

public class SymbolInteraction : MonoBehaviour
{
    [Header("Info Panel Content")]
    public string title;

    [TextArea(3, 10)]
    public string description;

    public Sprite image;

    [Header("References")]
    public InfoPanelController infoPanel;
    public SymbolAudio symbolAudio;

    void OnMouseDown()
    {
        if (symbolAudio != null)
            symbolAudio.PlaySymbolSound();

        infoPanel.Show(
            title,
            description,
            image
        );

        infoPanel.currentSymbolAudio = symbolAudio;
    }
}
