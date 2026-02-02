using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class SandOverlayBrushable : MonoBehaviour
{
    public float fadePerSecond = 0.35f;
    public bool disableWhenClear = true;

    private Renderer rend;
    private Material mat;
    private bool cleared = false;

    private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
    private static readonly int ColorID = Shader.PropertyToID("_Color");

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mat = rend.material; // instance material
    }

    public void Brush(float dt)
    {
        if (cleared || mat == null) return;

        // Read color from whichever property exists
        Color c;
        if (mat.HasProperty(BaseColorID)) c = mat.GetColor(BaseColorID);
        else if (mat.HasProperty(ColorID)) c = mat.GetColor(ColorID);
        else return;

        float newA = Mathf.Clamp01(c.a - fadePerSecond * dt);
        c.a = newA;

        // Write back
        if (mat.HasProperty(BaseColorID)) mat.SetColor(BaseColorID, c);
        else if (mat.HasProperty(ColorID)) mat.SetColor(ColorID, c);

        // Notify once when fully cleared
        if (newA <= 0.001f)
        {
            cleared = true;

            var handler = FindObjectOfType<YigalMosaicHandler>();
            if (handler != null)
                handler.NotifySandCleared();

            if (disableWhenClear)
                gameObject.SetActive(false);
        }
    }
}
