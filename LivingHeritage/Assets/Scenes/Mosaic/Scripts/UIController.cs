using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [Header("Introduction")]
    public GameObject introPanel;
    public TextMeshProUGUI scanTxt;

    [Header("Obvjective")]
    public GameObject objectivePanel;
    public GameObject tools;

    [Header("Instances")]
    public GameObject YigalInstance;

    private YigalMosaicHandler yigalHandler;

    public Stage stage;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        stage = Stage.PreScan;
        if (yigalHandler == null)
            yigalHandler = YigalInstance.GetComponent<YigalMosaicHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        FadePulse();
    }
    public void togglePanel(bool toggle) 
    {
        if (stage == Stage.PreScan)
        {
            introPanel.SetActive(toggle);
            scanTxt.gameObject.SetActive(!toggle);
            yigalHandler.gameStarted = true;
        }
        else if (stage == Stage.PostScan) { 
            objectivePanel.SetActive(toggle);
        }
    }

    private void FadePulse()
    {
        float speed = 1.5f;
        float minAlpha = 0.2f;
        float maxAlpha = 1f;
        bool fadingOut = true;

        if (scanTxt == null) return;

        Color c = scanTxt.color;

        if (fadingOut)
        {
            c.a -= speed * Time.deltaTime;

            if (c.a <= minAlpha)
            {
                c.a = minAlpha;
                fadingOut = false;
            }
        }
        else
        {
            c.a += speed * Time.deltaTime;

            if (c.a >= maxAlpha)
            {
                c.a = maxAlpha;
                fadingOut = true;
            }
        }

        scanTxt.color = c;
    }

    public void toggleObjectivePanel(bool toggle)
    {
        objectivePanel.SetActive(toggle);
    }

    public void toggleTools(bool toggle)
    {
        tools.SetActive(toggle);
    }
}
