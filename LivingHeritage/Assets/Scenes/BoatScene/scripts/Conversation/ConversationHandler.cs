using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversationHandler : MonoBehaviour
{

    public SubtitleManager subtitleManager;

    [Header("Male Clips")]
    public AudioClip male_Opening1;

    public AudioClip maleFoundNail1;
    public AudioClip maleOnNail1_2;
    public AudioClip maleOnNail1_4;

    public AudioClip maleOnNail2_1;
    public AudioClip maleOnNail2_3;

    public AudioClip maleOnPlank;

    public AudioClip male_OnMound1;

    public AudioClip male_dig;


    [Header("Female Clips")]
    public AudioClip female_Opening2;

    public AudioClip femaleOnNail1_1;
    public AudioClip femaleOnNail1_3;
    public AudioClip femaleOnNail1_5;

    public AudioClip femaleFoundNail2;
    public AudioClip femaleOnNail2_2;

    public AudioClip femaleFoundPlank;

    public AudioClip CheckTheMound;
    public AudioClip femaleOnMound2;

    public AudioClip femaleOnBoat;
    public AudioClip ending_female;


    public AudioSource maleSource;
    public AudioSource femaleSource;

    private AudioSequence sequence;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(startConversation());
    }

    // Update is called once per frame
    void Update()
    {
        sequence?.Update(Time.deltaTime);
    }

    private IEnumerator startConversation() {
        yield return new WaitForSeconds(3f);

        sequence = new AudioSequence(maleSource, subtitleManager, new List<AudioClip> { male_Opening1, female_Opening2}, 0.5f);
        sequence.Play();
    }
    public IEnumerator playClip(AudioClip audioClip)
    {
        //while (maleSource.isPlaying || femaleSource.isPlaying)
        //    yield return null;
        //source.clip = audioClip;
        //source.Play();
        yield return new WaitForSeconds(1f);
        sequence = new AudioSequence(maleSource, subtitleManager, new List<AudioClip> { audioClip }, 0.5f);
        sequence.Play();
    }

    public void forceStop() {
        StopAllCoroutines();
        maleSource.Stop();
        femaleSource.Stop();
    }

    public IEnumerator playNail1Dialogue() {
        yield return new WaitForSeconds(1f);

        List<AudioClip> clips = new List<AudioClip> { femaleOnNail1_1, maleOnNail1_2 , femaleOnNail1_3 ,
        maleOnNail1_4, femaleOnNail1_5};
        sequence = new AudioSequence(maleSource, subtitleManager, clips, 0.5f);
        sequence.Play();

        //yield return StartCoroutine(playClip(femaleSource, femaleOnNail1_1));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(maleSource, maleOnNail1_2));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(femaleSource, femaleOnNail1_3));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(maleSource, maleOnNail1_4));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(femaleSource, femaleOnNail1_5));
        //yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator plankFollowUp() {
        yield return new WaitForSeconds(1f);

        List<AudioClip> clips = new List<AudioClip> {maleOnPlank, male_OnMound1, CheckTheMound };
        sequence = new AudioSequence(maleSource, subtitleManager, clips, 0.5f);
        sequence.Play();

        //StartCoroutine(playClip(maleSource, maleOnPlank));
        //yield return new WaitForSeconds(4f);
        //yield return StartCoroutine(playClip(maleSource, male_OnMound1));
        //yield return new WaitForSeconds(2f);
        //yield return StartCoroutine(playClip(femaleSource, CheckTheMound));

    }

    public IEnumerator playNail2Dialogue()
    {
        yield return new WaitForSeconds(1f);


        List<AudioClip> clips = new List<AudioClip> {maleOnNail2_1, femaleOnNail2_2, maleOnNail2_3 };
        sequence = new AudioSequence(maleSource, subtitleManager, clips, 0.5f);
        sequence.Play();

        //yield return StartCoroutine(playClip(maleSource, maleOnNail2_1));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(femaleSource, femaleOnNail2_2));
        //yield return new WaitForSeconds(0.5f);
        //yield return StartCoroutine(playClip(maleSource, maleOnNail2_3));
        //yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator playMoundClips() {
        yield return new WaitForSeconds(1f);

        List<AudioClip> clips = new List<AudioClip> { femaleOnMound2 };
        sequence = new AudioSequence(maleSource, subtitleManager, clips, 0.5f);
        sequence.Play();
        
        //yield return StartCoroutine(playClip(femaleSource, femaleOnMound2));

    }
}
