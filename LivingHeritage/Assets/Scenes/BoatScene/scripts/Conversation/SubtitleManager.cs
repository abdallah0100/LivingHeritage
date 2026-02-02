using TMPro;
using UnityEngine;

public class SubtitleManager : MonoBehaviour
{
    [Header("References")]
    public AudioSource audioSource;
    public TextMeshProUGUI subtitleText;
    public SubtitleLibrary library;
    public GameObject subtitleArea;

    [Header("Language")]
    public string currentLang = "en";
    public string fallbackLang = "en";

    private SubtitleTrack track;
    private int cueIndex;

    void Start()
    {
        subtitleText.text = "";
        LoadForCurrentClip();
    }

    void Update()
    {
        if (audioSource == null || !audioSource.isPlaying || track == null || track.cues == null)
        {
            subtitleText.text = "";
            subtitleArea.SetActive(false);
            return;
        }

        float t = audioSource.time;

        while (cueIndex < track.cues.Count && t > track.cues[cueIndex].end)
            cueIndex++;

        if (cueIndex >= track.cues.Count)
        {
            subtitleText.text = "";
            subtitleArea.SetActive(false);
            return;
        }

        var cue = track.cues[cueIndex];

        if (t >= cue.start && t <= cue.end)
        {
            subtitleText.text = GetCueText(cue, currentLang, fallbackLang);
            subtitleArea.SetActive(true);
        }
        else
        {
            subtitleText.text = "";
            subtitleArea.SetActive(false);
        }
    }

    public void LoadForCurrentClip()
    {
        track = null;
        cueIndex = 0;
        subtitleText.text = "";
        subtitleArea.SetActive(false);

        if (audioSource == null || library == null) return;

        TextAsset json = library.GetJson(audioSource.clip);
        if (json == null) return;

        track = JsonUtility.FromJson<SubtitleTrack>(json.text);
    }

    public void OnNewClipStarted()
    {
        LoadForCurrentClip();
    }

    static string GetCueText(SubtitleCue cue, string lang, string fallback)
    {
        if (cue.lines == null) return "";

        foreach (var l in cue.lines)
            if (l.lang == lang) return l.text;

        foreach (var l in cue.lines)
            if (l.lang == fallback) return l.text;

        return cue.lines.Count > 0 ? cue.lines[0].text : "";
    }
}
