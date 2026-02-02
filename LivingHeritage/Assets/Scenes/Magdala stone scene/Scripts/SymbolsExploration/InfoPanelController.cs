using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfoPanelController : MonoBehaviour
{
    [Header("Overlay + Window")]
    public CanvasGroup overlayGroup;   
    public CanvasGroup windowGroup;    

    [Header("UI Refs")]
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI bodyText;
    public Image photoImage;

    [Header("Buttons")]
    public Button closeButton;
    public Button actionButton;
    public TextMeshProUGUI actionButtonLabel;

    [Header("Fade Settings")]
    public float fadeDuration = 0.25f;

    private Coroutine routine;

    private Action _onCloseCallback;
    private Action _onActionCallback;

    private void Awake()
    {
        HideInstant();

        if (closeButton != null)
            closeButton.onClick.AddListener(Close);

        if (actionButton != null)
            actionButton.onClick.AddListener(OnActionClicked);
    }

    public void Show(string title,
                     string body,
                     Sprite photo = null,
                     string actionText = null,
                     Action onCloseCallback = null,
                     Action onActionCallback = null)
    {
        _onCloseCallback = onCloseCallback;
        _onActionCallback = onActionCallback;

        titleText.text = title;
        bodyText.text = body;

        if (photo != null)
        {
            photoImage.sprite = photo;
            photoImage.gameObject.SetActive(true);
        }
        else
        {
            photoImage.gameObject.SetActive(false);
        }

        if (actionButton != null)
        {
            bool useButton = !string.IsNullOrEmpty(actionText) || onActionCallback != null;
            actionButton.gameObject.SetActive(useButton);

            if (useButton)
                actionButtonLabel.text = actionText ?? "Continue";
        }

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeIn());
    }

    public SymbolAudio currentSymbolAudio;
    public void Close()
    {
        if (currentSymbolAudio != null)
        {
            currentSymbolAudio.StopSound();
            currentSymbolAudio = null;
        }

        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(FadeOut());
    }


    private void OnActionClicked()
    {
        Close();

        _onActionCallback?.Invoke();
        _onActionCallback = null;
    }

    private IEnumerator FadeIn()
    {
        overlayGroup.blocksRaycasts = true;
        windowGroup.blocksRaycasts = true;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);

            overlayGroup.alpha = Mathf.Lerp(0, 0.5f, k);
            windowGroup.alpha = Mathf.Lerp(0, 1f, k);

            yield return null;
        }
    }

    private IEnumerator FadeOut()
    {
        float overlayStart = overlayGroup.alpha;
        float windowStart = windowGroup.alpha;

        float t = 0;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float k = Mathf.Clamp01(t / fadeDuration);

            overlayGroup.alpha = Mathf.Lerp(overlayStart, 0, k);
            windowGroup.alpha = Mathf.Lerp(windowStart, 0, k);

            yield return null;
        }

        HideInstant();

        _onCloseCallback?.Invoke();
        _onCloseCallback = null;
    }

    private void HideInstant()
    {
        overlayGroup.alpha = 0;
        windowGroup.alpha = 0;

        overlayGroup.blocksRaycasts = false;
        windowGroup.blocksRaycasts = false;
    }
}
