using UnityEngine;

public class AchievementHandler : MonoBehaviour
{

    [Header("Badge Check Icon")]
    public GameObject sharpEye;
    public GameObject storySeeker;
    public GameObject recordBreaker;
    public GameObject speedRunner;
    public GameObject returner;
    public GameObject perfectRun;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        CheckSharpEye();
        checkStorySeeker();
        checkRecordBreaker();
        checkSpeedRunner();
        checkReturner();
        checkPerfectRun();

        toggleBadges();
    }

    public void CheckSharpEye() {

        if (GameStats.wasAssisted)
            return;
        if (GameStats.gameEnded)
            GameStats.sharpEye = true;

    }

    public void checkStorySeeker() {
        if (!GameStats.gameEnded)
            return;
        if (GameStats.wentThroughAllDialogues)
            GameStats.storySeeker = true;
    }

    public void checkRecordBreaker() {
        if (!GameStats.gameEnded)
            return;
        if (SessionVariables.fastestTime >= 0) // can be 0 if no value exist in the database
        {
            if (SessionVariables.user.overAllTime <= SessionVariables.fastestTime)
                GameStats.recordBreaker = true;
        }
    }

    public void checkSpeedRunner()
    {
        return; // TODO
    }

    public void checkReturner() { 
        if (GameStats.replayAfterCompleting)
            GameStats.theReturner = true;
    }

    public void checkPerfectRun()
    {
        if (!GameStats.gameEnded)
            return;
        if (!GameStats.wasAssisted && SessionVariables.user.overAllTime <= SessionVariables.fastestTime)
            GameStats.perfectRun = true;
    }


    public void toggleBadges() 
    {
        sharpEye.SetActive(GameStats.sharpEye);
        storySeeker.SetActive(GameStats.storySeeker);
        recordBreaker.SetActive(GameStats.recordBreaker);
        speedRunner.SetActive(GameStats.speedRunner);
        returner.SetActive(GameStats.theReturner);
        perfectRun.SetActive(GameStats.perfectRun);
    }
}
