using UnityEngine;

public class ScreenOrientationLock : MonoBehaviour
{
    void Start()
    {
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.autorotateToPortrait = false;
        Screen.autorotateToPortraitUpsideDown = false;

        Screen.orientation = ScreenOrientation.LandscapeLeft;

        Invoke("ForceLandscape", 0.5f);
    }

    void ForceLandscape()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }
}
