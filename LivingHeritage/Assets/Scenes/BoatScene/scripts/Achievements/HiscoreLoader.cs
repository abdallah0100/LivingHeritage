using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class HiscoreLoader : MonoBehaviour
{
    [Header("Top Scorers Text Fields")]
    public TextMeshProUGUI extraField;
    public TextMeshProUGUI[] topScorers;

    [Header("Panels")]
    public GameObject HiScorePanel;
    public GameObject scorePanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        loadHiScores();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private async void loadHiScores() {
        double userTime = SessionVariables.user.overAllTime;
        List<User>? users = await FirebaseRealtimeService.Instance.GetAllUsers();
        if (users == null || users.Count == 0)
            return;
        List<User> orderedUsers = // order the list ascending
            users.OrderBy(u => u.overAllTime).ToList();

        for (int i = 0; i < orderedUsers.Count; i++) {
            if (i >= 5 || i >= topScorers.Length)
                break;
            topScorers[i].SetText("#" + (i + 1) + " " + orderedUsers[i].name + " Time: " + BeachMission.timeToString(orderedUsers[i].overAllTime, 1));
        
        }
        int playerRank = 0;
        for (int i = 0; i < orderedUsers.Count; i++)
        {
            if (orderedUsers[i].name == SessionVariables.user.name && orderedUsers[i].overAllTime == userTime)
            {
                playerRank = i;
                break;
            }
        }

        extraField.SetText("You placed at #" + (playerRank + 1) + "!");
    }

    public void back()
    {
        HiScorePanel.SetActive(false);
        scorePanel.SetActive(true);
    }
}
