using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour
{
    [Header("Component Assigning")]
    public TMP_InputField nameInput;
    public TMP_InputField ageInput;
    public Button maleButton;
    public Button femaleButton;
    public TextMeshProUGUI outputMsg;
    public TextMeshProUGUI expTxt;

    private string name;
    private int age = -1;
    private bool male;
    private string expLevel;
    private bool selectedGender = false;
    private Color32 grey = new Color32(0xD0, 0xD0, 0xD0, 0xFF);


     void Start()
    {
        ScreenOrientation orientation = ScreenOrientation.Portrait;
        Screen.orientation = orientation;
    }

    // Update is called once per frame
    void Update()
    {
        name = nameInput.text;
        if (ageInput.text.Length > 0)
            age = int.Parse(ageInput.text);
        expLevel = expTxt.text;
    }

    public void continueClick() {
        if (name.Length == 0)
            sendErrMsg("Please enter your name first.");
        else if (age <= 3 || age > 90)
            sendErrMsg("Please enter a valid age. (4-90)");
        else if (!selectedGender)
            sendErrMsg("Please select your gender.");
        else { 
            outputMsg.gameObject.SetActive(false);
            registerUser();
        }
    }

    private /*async*/ void registerUser()
    {
        User user = new User(name, age, male, expLevel);
        SessionVariables.user = user;
        loadArtifactSelection();
        //bool success = await FirebaseRealtimeService.Instance.AddUser(user);

        //if (success)
        //{
        //    loadArtifactSelection();
        //}
        //else
        //{
        //    sendErrMsg("Something went wrong registering...");
        //}
    }

    public void loadArtifactSelection() {
        SceneManager.LoadScene("ArtifactSelection");
    }


    public void setFemale()
    {
        selectedGender = true;
        Color32 pinkish = new Color32(215, 90, 164, 255);

        // Female button -> pink
        ColorBlock cb = femaleButton.colors;
        cb.normalColor = pinkish;
        cb.highlightedColor = pinkish;
        cb.selectedColor = pinkish;
        femaleButton.colors = cb;

        male = false;

        // Male button -> grey
        cb = maleButton.colors;
        cb.normalColor = grey;
        cb.highlightedColor = grey;
        cb.selectedColor = grey;
        maleButton.colors = cb;
    }

    public void setMale()
    {
        selectedGender = true;
        Color32 blueish = new Color32(24, 103, 166, 255);

        // Male button -> blue
        ColorBlock cb = maleButton.colors;
        cb.normalColor = blueish;
        cb.highlightedColor = blueish;
        cb.selectedColor = blueish;
        maleButton.colors = cb;

        male = true;

        // Female button -> grey
        cb = femaleButton.colors;
        cb.normalColor = grey;
        cb.highlightedColor = grey;
        cb.selectedColor = grey;
        femaleButton.colors = cb;
    }


    private void sendErrMsg(string msg) {
        outputMsg.SetText("* " + msg);
        outputMsg.color = Color.red;
        outputMsg.gameObject.SetActive(true);

    }
}
