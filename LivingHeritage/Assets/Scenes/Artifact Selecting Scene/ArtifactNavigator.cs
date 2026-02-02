using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactNavigator : MonoBehaviour
{
    [Header("Exhibits (UI holders under Canvas)")]
    public GameObject[] targets;

    [Header("Panel Assignments")]
    public Canvas infoPanel;
    public TextMeshProUGUI exhibitName;
    public TextMeshProUGUI exhibitDescription;

    [Header("Swipe Animation")]
    public float slideDuration = 0.35f;
    public float slideDistance = 900f; // pixels (tune per resolution)

    private int currentIndex = 0;
    private bool isAnimating = false;
    private Vector2 centerPos;

    private string selectedScene = "None";

    private Dictionary<string, string[]> exhibitInfo = new Dictionary<string, string[]>() {
        { "boat",
            new string[] { "The Ancient Boat", "In 1986 two brothers from Kibbutz" +
                " Ginosar discovered the remains of an ancient boat on the northwest shore of the sea of the Gallilee. " +
                "Their journey is presented as a minigame where you walk along the beach, discover the scattered remains, and ultimately uncover the ancient boat " }
        },
        { "stone",
            new string[]{"The Magdala Stone", "This limestone stone, carved with symbols" +
                " and architectural elements resembling the Jerusalem Temple, served for reading the Torah." +
                " Its front bears the oldest synagogue depiction of the Temple lamp. It originally stood at the center of the first-century" +
                " Migdal synagogue on the shores of the Sea of Galilee, when the Temple still existed." }
        },
        { "mosaic",
            new string[]{"The Ancient Mosaic", "Excavations at Hukok since 2011 revealed a" +
                " 5th-century CE synagogue with elaborate mosaics. This scene shows Samson carrying the gates of Gaza" +
                " (Judges 16:1–3). The mosaic has not been fully restored yet." }
        }
    };

    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;

        // Find which one is currently active (boat should be active)
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null && targets[i].activeSelf)
            {
                currentIndex = i;
                break;
            }
        }

        // Cache the "center" anchoredPosition from the currently active target
        RectTransform activeRT = targets[currentIndex].GetComponent<RectTransform>();
        if (activeRT != null)
            centerPos = activeRT.anchoredPosition;

        // Ensure only one is visible and all are at center position initially
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] == null) continue;

            targets[i].SetActive(i == currentIndex);

            RectTransform rt = targets[i].GetComponent<RectTransform>();
            if (rt != null)
                rt.anchoredPosition = centerPos;
        }
    }

    public void loadArtifactScene()
    {
        if (selectedScene.Equals("None"))
            return;

        SceneManager.LoadScene(selectedScene);
    }

    private void openInfoPanel(string sceneName)
    {
        string title = exhibitInfo[sceneName][0];
        string description = exhibitInfo[sceneName][1];

        exhibitName.SetText(title);
        exhibitDescription.SetText(description);
        infoPanel.gameObject.SetActive(true);
    }

    public void closeInfoPanel()
    {
        infoPanel.gameObject.SetActive(false);
    }

    public void Next()
    {
        if (isAnimating) return;

        int nextIndex = (currentIndex + 1) % targets.Length;

        // current slides left, next comes from right
        StartCoroutine(SlideTransition(currentIndex, nextIndex, incomingFromRight: true));
        currentIndex = nextIndex;
    }

    public void Previous()
    {
        if (isAnimating) return;

        int prevIndex = (currentIndex - 1 + targets.Length) % targets.Length;

        // current slides right, prev comes from left
        StartCoroutine(SlideTransition(currentIndex, prevIndex, incomingFromRight: false));
        currentIndex = prevIndex;
    }

    private IEnumerator SlideTransition(int fromIndex, int toIndex, bool incomingFromRight)
    {
        isAnimating = true;

        GameObject fromObj = targets[fromIndex];
        GameObject toObj = targets[toIndex];

        if (fromObj == null || toObj == null)
        {
            // fallback
            ShowOnly(currentIndex);
            isAnimating = false;
            yield break;
        }

        RectTransform fromRT = fromObj.GetComponent<RectTransform>();
        RectTransform toRT = toObj.GetComponent<RectTransform>();

        if (fromRT == null || toRT == null)
        {
            // fallback (in case someone forgot RectTransform)
            ShowOnly(currentIndex);
            isAnimating = false;
            yield break;
        }

        float dir = incomingFromRight ? 1f : -1f;

        // activate incoming and place it off-screen
        toObj.SetActive(true);
        toRT.anchoredPosition = centerPos + new Vector2(dir * slideDistance, 0f);

        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / slideDuration;
            float s = Smooth01(t);

            // outgoing goes opposite direction
            fromRT.anchoredPosition = Vector2.Lerp(
                centerPos,
                centerPos - new Vector2(dir * slideDistance, 0f),
                s
            );

            // incoming comes from dir side to center
            toRT.anchoredPosition = Vector2.Lerp(
                centerPos + new Vector2(dir * slideDistance, 0f),
                centerPos,
                s
            );

            yield return null;
        }

        // finalize
        fromObj.SetActive(false);
        fromRT.anchoredPosition = centerPos;
        toRT.anchoredPosition = centerPos;

        isAnimating = false;
    }

    private float Smooth01(float t)
    {
        // smoothstep easing
        t = Mathf.Clamp01(t);
        return t * t * (3f - 2f * t);
    }

    private void ShowOnly(int indexToShow)
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
                targets[i].SetActive(i == indexToShow);

            RectTransform rt = targets[i] != null ? targets[i].GetComponent<RectTransform>() : null;
            if (rt != null)
                rt.anchoredPosition = centerPos;
        }
    }

    public void ShowOverView()
    {
        string exhibit = currentIndex == 0 ? "boat" : currentIndex == 1 ? "stone" : "mosaic";
        selectedScene = currentIndex == 0 ? "BoatMainScene" : currentIndex == 1 ? "MagdalaStone" : "MosaicStartScene";
        openInfoPanel(exhibit);
    }
}
