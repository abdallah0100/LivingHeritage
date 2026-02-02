using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardButtonScripts : MonoBehaviour
{
    [Header("Panels")]
    public GameObject HiScorePanel;
    public GameObject scorePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void loadScene(string sceneName)
    {
        if (sceneName.Equals("BoatMainScene"))
            GameStats.theReturner = true;

        SceneManager.LoadScene(sceneName);
    }
    public void openHighscores() {
        HiScorePanel.SetActive(true);
        scorePanel.SetActive(false);
    }
}
