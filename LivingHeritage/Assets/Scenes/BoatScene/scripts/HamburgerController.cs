using UnityEngine;
using UnityEngine.SceneManagement;

public class HamburgerController : MonoBehaviour
{
    public GameObject hamBurgerPanel;

    [Header("Main Menu")]
    public GameObject muteObject;
    public GameObject soundOnObject;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        toggleSound(AudioListener.volume > 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleSound(bool sound)
    {
        muteObject.SetActive(!sound);
        soundOnObject.SetActive(sound);

        AudioListener.volume = sound ? 1.0f : 0.0f;
    }

    public void resumeGame() { 
        hamBurgerPanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void openHamBurger() {
        hamBurgerPanel.SetActive(!hamBurgerPanel.active);
    }
    public void restartGame(string scene) {
        SceneManager.LoadScene(scene);
    }

    public void navToHome() {
        SceneManager.LoadScene("ArtifactSelection");
    }

}
