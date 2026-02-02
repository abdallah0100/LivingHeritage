using UnityEngine;

public static class GameStats
{
    public static bool gameEnded = false;
    public static bool wasAssisted = false;
    public static double finishTime = 0;
    public static bool wentThroughAllDialogues = true;
    public static bool replayAfterCompleting = false;

    public static bool sharpEye = false;
    public static bool storySeeker = false;
    public static bool recordBreaker = false;
    public static bool speedRunner = false;
    public static bool theReturner = false;
    public static bool perfectRun = false;

    public static void reset() {
        Debug.Log("Resetting stats...");
        gameEnded = false;
        wasAssisted = false;
        finishTime = 0;
        wentThroughAllDialogues = true;
        replayAfterCompleting = false;
    }
}
