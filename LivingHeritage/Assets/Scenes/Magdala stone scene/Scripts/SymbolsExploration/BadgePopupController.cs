using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class BadgePopupController : MonoBehaviour
{
    public static BadgePopupController Instance;

    [Header("UI")]
    public CanvasGroup canvasGroup;
    public Image badgeIcon;
    public TextMeshProUGUI badgeText;

    [Header("Timing")]
    public float fadeDuration = 0.25f;
    public float visibleDuration = 1.8f;

    private Coroutine routine;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        HideInstant();
    }

    public void Show(string title, Sprite icon)
    {
        if (routine != null)
            StopCoroutine(routine);

        badgeText.text = title;
        badgeIcon.sprite = icon;

        routine = StartCoroutine(ShowRoutine());
    }

    IEnumerator ShowRoutine()
    {
        yield return Fade(0, 1);
        yield return new WaitForSeconds(visibleDuration);
        yield return Fade(1, 0);
    }

    IEnumerator Fade(float from, float to)
    {
        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, t / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }

    void HideInstant()
    {
        canvasGroup.alpha = 0;
    }
}
