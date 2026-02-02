using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{
    private int score = 0;

    [Header("Quiz Data")]
    public QuizQuestion[] questions;
    private int currentIndex = 0;
    private bool answered = false;

    [Header("UI")]
    public TextMeshProUGUI questionText;
    public Button[] answerButtons;
    public TextMeshProUGUI feedbackText;

    [Header("Summary UI")]
    public GameObject summaryPanel;
    public TextMeshProUGUI scoreText;


    Color32 correctColor = new Color32(46, 204, 113, 255); // strong green
    Color32 wrongColor = new Color32(255, 0, 0, 255); // strong red

    Color defaultColor;

    void Start()
    {
        defaultColor = answerButtons[0].image.color;
        ShuffleQuestions();
        ShowQuestion();
    }

    void ShuffleQuestions()
    {
        for (int i = 0; i < questions.Length; i++)
        {
            int randomIndex = Random.Range(i, questions.Length);
            QuizQuestion temp = questions[i];
            questions[i] = questions[randomIndex];
            questions[randomIndex] = temp;
        }
    }

    void ShowQuestion()
    {
        answered = false;
        feedbackText.text = "";

        QuizQuestion q = questions[currentIndex];
        questionText.text = q.question;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int index = i;

            Button btn = answerButtons[i];
            btn.interactable = true;
            btn.image.color = defaultColor;

            btn.GetComponentInChildren<TextMeshProUGUI>().text =
                q.answers[i];

            btn.onClick.RemoveAllListeners();
            btn.onClick.AddListener(() => SelectAnswer(index, btn));
        }
    }

    void SelectAnswer(int index, Button clickedButton)
    {
        if (answered)
            return;

        answered = true;

        foreach (Button b in answerButtons)
            b.interactable = false;

        if (index == questions[currentIndex].correctIndex)
        {
            clickedButton.image.color = correctColor;
            feedbackText.text = "Correct";
            score++; // count correct answer
        }
        else
        {
            clickedButton.image.color = wrongColor;
            feedbackText.text = "Wrong";
        }

        Invoke(nameof(NextQuestion), 1.5f);
    }


    void NextQuestion()
    {
        currentIndex++;

        if (currentIndex >= questions.Length)
        {
            ShowSummary();
            return;
        }

        ShowQuestion();
    }

    void ShowSummary()
    {
        // Hide quiz UI
        questionText.gameObject.SetActive(false);
        feedbackText.gameObject.SetActive(false);

        foreach (Button b in answerButtons)
            b.gameObject.SetActive(false);

        // Show summary
        summaryPanel.SetActive(true);

        scoreText.text =
            $"You got {score} out of {questions.Length} correct. Well done!";

    }


    public void GoBackToStone()
    {
        SceneManager.LoadScene("StoneExplorationScene");
    }



}
