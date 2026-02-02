using UnityEngine;

public class OpenQuizLink : MonoBehaviour
{
    [SerializeField]
    private string quizURL = "https://docs.google.com/forms/d/e/1FAIpQLSf5ai-GMK_z3ED7JAwbYoey_8R3OVH6vi1CkU0uM3y1DkPX-g/viewform";

    public void OpenQuiz()
    {
        Application.OpenURL(quizURL);
    }
}
