using UnityEngine;
using TMPro;


public class NoScanInitiator : MonoBehaviour
{
    public GameObject yigal;
    public TextMeshProUGUI subtitlesText;

    void Start()
    {
        var speech = yigal.GetComponent<YigalSpeech>();
        if (speech != null)
        {
            speech.setSubTitleComp(subtitlesText);
            speech.mosaicTransform = transform;
            speech.StartYigalSpeech();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
