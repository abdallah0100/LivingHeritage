using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class LandingButtonHandler : MonoBehaviour
{
    [Header("Connection Status")]
    public Image status;

    private void Start()
    {
        status.color = Color.red;
        ScreenOrientation orientation = ScreenOrientation.Portrait;
        Screen.orientation = orientation;
    }

    private void Update()
    {
        if (FirebaseRealtimeService.Initialized)
            status.color = Color.green;
        else
            status.color = Color.red;
    }
    public void loadArtifactSelection() { 
        SceneManager.LoadScene("UserInputScene");
    }
}
