using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenQuizScene : MonoBehaviour
{
    public void OpenQuiz()
    {
        SceneManager.LoadScene("StoneQuizScene");
    }
}
