using UnityEngine;

public class QuizHandler : MonoBehaviour
{

    public GameObject mainPanel;

    public GameObject finishedTasks;
    public GameObject didnotFinishTasks;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (SessionVariables.completedMosaic && SessionVariables.completedStone && SessionVariables.completedBoat)
        {
            finishedTasks.SetActive(true);
            didnotFinishTasks.SetActive(false);
        }
        else
        {
            finishedTasks.SetActive(false);
            didnotFinishTasks.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void close()
    {
        mainPanel.SetActive(false);
    }

    public void openPanel() { 
        mainPanel.SetActive(true);
    }
}
