using TMPro;
using UnityEngine;

public class BoatUIController : MonoBehaviour
{
    [Header("Intro Components")]
    public GameObject introMainPanel;
    public GameObject panel1;
    public GameObject panel2;

    [Header("Objective Components")]
    public GameObject collapsedPanel;
    public GameObject expandedPanel;

    public TextMeshProUGUI nailsFound;
    public TextMeshProUGUI planksFound;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 0f; // pause the game
        AudioListener.volume = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void openIntroPanel() 
    {
        introMainPanel.SetActive(true);
    }

    public void nextPanel() {
        panel1.SetActive(false);
        panel2.SetActive(true);
    }
    public void prevPanel() 
    {
        panel1.SetActive(true);
        panel2.SetActive(false);
    }

    public void toggleObjectivePanel(bool expand)
    {
        collapsedPanel.SetActive(!expand);
        expandedPanel.SetActive(expand);
    }

    public void startGame()
    {
        Time.timeScale = 1f;
        introMainPanel.SetActive(false);
    }

    public void updateProgress(int foundNails, int totalNails, int foundPlanks, int totalPlanks) 
    {
        string nailsTxt1 = foundNails + "/" + totalNails;
        string plankTxt1 = foundPlanks + "/" + totalPlanks;

        string finalNailsTxt = foundNails == totalNails ? "<s>" + nailsTxt1 + "</s>" : nailsTxt1;
        string finalPlankTxt = foundPlanks == totalPlanks ? "<s>" + plankTxt1 + "</s>" : plankTxt1;
        nailsFound.SetText(finalNailsTxt);
        planksFound.SetText(finalPlankTxt);
    }
}
