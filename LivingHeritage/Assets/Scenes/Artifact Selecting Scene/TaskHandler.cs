using UnityEngine;
using UnityEngine.UI;

public class TaskHandler : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject expandedTaskPanel;

    public Sprite complete;
    public Sprite inComplete;

    public Image boatProgress;
    public Image stoneProgress;
    public Image mosaicProgress;

    void Start()
    {
        boatProgress.sprite = SessionVariables.completedBoat ? complete : inComplete;
        stoneProgress.sprite = SessionVariables.completedStone ? complete : inComplete;
        mosaicProgress.sprite = SessionVariables.completedMosaic ? complete : inComplete;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleTaskPanel(bool on)
    {
        expandedTaskPanel.SetActive(on);
    }
}
