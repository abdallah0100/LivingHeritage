using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public class StoneSideQuestion
{
    public string sideName;
    public Sprite[] optionSprites = new Sprite[3];
    [Range(0, 2)]
    public int correctIndex;
}

public class StoneQuizManager : MonoBehaviour
{
    [Header("Questions (3–4 sides)")]
    public StoneSideQuestion[] sides;

    [Header("UI References")]
    public TextMeshProUGUI questionLabel;
    public Image[] optionImages;

    [Header("Result UI")]
    public GameObject resultPanel;

    public TextMeshProUGUI correctText;
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI scoreText;

    [Header("Stars System")]
    public GameObject starPrefab;   
    public Transform starsParent;   
    public Sprite starOn;           
    public Sprite starOff;         

    int currentSideIndex = 0;
    int correctCount = 0;
    float startTime;
    bool quizFinished = false;

    void Start()
    {
        startTime = Time.time;
        if (sides != null && sides.Length > 0)
            LoadSide(0);
    }

    void LoadSide(int index)
    {
        var q = sides[index];

        if (questionLabel != null)
            questionLabel.text = $"Choose the correct layout for: {q.sideName}";

        for (int i = 0; i < optionImages.Length; i++)
        {
            if (optionImages[i] != null && i < q.optionSprites.Length)
            {
                optionImages[i].sprite = q.optionSprites[i];
                optionImages[i].enabled = (q.optionSprites[i] != null);
            }
        }
    }

    // Called by buttons (0,1,2)
    public void OnOptionChosen(int optionIndex)
    {
        if (quizFinished) return;

        var q = sides[currentSideIndex];

        if (optionIndex == q.correctIndex)
            correctCount++;

        currentSideIndex++;

        if (currentSideIndex < sides.Length)
        {
            LoadSide(currentSideIndex);
        }
        else
        {
            FinishQuiz();
        }
    }

    void FinishQuiz()
    {
        quizFinished = true;

        float puzzleTime = Time.time - startTime;

        float correctness = (float)correctCount / sides.Length;
        float timeFactor = Mathf.Clamp01(60f / (puzzleTime + 1f));
        float totalScore = (correctness * 0.7f + timeFactor * 0.3f) * 100f;

        if (resultPanel != null)
            resultPanel.SetActive(true);

        if (correctText != null)
            correctText.text = $"Correct sides: {correctCount}/{sides.Length}";

        if (timeText != null)
            timeText.text = $"Time: {puzzleTime:F1} sec";

        if (scoreText != null)
            scoreText.text = $"Score: {totalScore:F0}";

        // 🔥 Generate stars according to score
        int stars = GetStarCount((int)totalScore);
        GenerateStars(stars);
    }



    int GetStarCount(int score)
    {
        if (score >= 95) return 5;
        if (score >= 80) return 4;
        if (score >= 60) return 3;
        if (score >= 40) return 2;
        if (score >= 20) return 1;
        return 0;
    }


    void GenerateStars(int count)
    {
        foreach (Transform child in starsParent)
            Destroy(child.gameObject);

        for (int i = 0; i < 5; i++)
        {
            GameObject star = Instantiate(starPrefab, starsParent);

            Image img = star.GetComponent<Image>();

            img.sprite = i < count ? starOn : starOff;
        }
    }

    // -------------------------------------------------------
    // Buttons Actions
    // -------------------------------------------------------
    public void RetryQuiz()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitQuiz()
    {
        SceneManager.LoadScene("StartScene");
    }
}
