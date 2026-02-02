using System.Collections;
using UnityEngine;

public class RubbleCleanable : MonoBehaviour
{
    public float fadeDuration = 0.25f;

    private SpriteRenderer sr;
    private bool cleaned;

    public System.Action<RubbleCleanable> onCleaned; // assigned by spawner

    void Awake()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    void OnEnable()
    {
        cleaned = false;

        if (sr == null)
            sr = GetComponentInChildren<SpriteRenderer>();

        if (sr != null)
        {
            Color c = sr.color;
            sr.color = new Color(c.r, c.g, c.b, 1f);
        }
    }


    // Called by the manager when user taps this rubble
    public void Clean()
    {
        if (cleaned) return;
        cleaned = true;
        StartCoroutine(FadeOutAndDisable());
    }

    private IEnumerator FadeOutAndDisable()
    {
        if (sr == null)
        {
            gameObject.SetActive(false);
            onCleaned?.Invoke(this);
            yield break;
        }

        Color c = sr.color;
        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            sr.color = new Color(c.r, c.g, c.b, a);
            yield return null;
        }

        gameObject.SetActive(false);
        onCleaned?.Invoke(this);
    }
}
