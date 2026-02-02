using UnityEngine;
using UnityEngine.SceneManagement;

public class StoneRebuildTrigger : MonoBehaviour
{
    [Header("Info Panel hook")]
    [SerializeField] private InfoPanelController infoPanel;
    [SerializeField] private Sprite stonePhoto;

    [Header("Scene")]
    [SerializeField] private string explorationSceneName = "StoneExplorationScene";

    [TextArea]
    public string panelTitle = "Exploration Complete";

    [TextArea(3, 10)]
    public string panelBody =
        "You have explored all the symbols of the Magdala Stone.\n\n" +
        "You can now continue exploring the stone freely.";

    private bool openedOnce = false;

    private void OnTriggerEnter(Collider other)
    {
        if (openedOnce) return;
        if (!other.CompareTag("Player")) return;

        openedOnce = true;

        infoPanel.Show(
            panelTitle,
            panelBody,
            stonePhoto,
            actionText: "Continue Exploring",
            null,
            onActionCallback: () =>
            {
                SceneManager.LoadScene(explorationSceneName);
            }
        );
    }
}
