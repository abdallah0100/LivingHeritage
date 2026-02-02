using System.Collections.Generic;
using UnityEngine;

public class AudioSequence
{
    private readonly AudioSource audioSource;
    private readonly SubtitleManager subtitleManager;
    private readonly List<AudioClip> clips;
    private readonly float delayBetweenClips;

    private int index = -1;
    private bool isPlaying = false;
    private bool waitingForNext = false;
    private float delayTimer = 0f;

    public AudioSequence(
        AudioSource audioSource,
        SubtitleManager subtitleManager,
        List<AudioClip> clips,
        float delayBetweenClips = 1f)
    {
        this.audioSource = audioSource;
        this.subtitleManager = subtitleManager;
        this.clips = clips;
        this.delayBetweenClips = delayBetweenClips;
    }

    public void Play()
    {
        if (clips == null || clips.Count == 0 || audioSource == null)
            return;

        index = 0;
        isPlaying = true;
        waitingForNext = false;

        PlayCurrent();
    }

    public void Update(float deltaTime)
    {
        if (!isPlaying || audioSource == null)
            return;

        // Clip finished, start silence delay
        if (!audioSource.isPlaying && !waitingForNext)
        {
            waitingForNext = true;
            delayTimer = delayBetweenClips;
        }

        // Silence countdown
        if (waitingForNext)
        {
            delayTimer -= deltaTime;

            if (delayTimer <= 0f)
            {
                waitingForNext = false;
                index++;

                if (index < clips.Count)
                {
                    PlayCurrent();
                }
                else
                {
                    isPlaying = false; // sequence finished
                }
            }
        }
    }

    private void PlayCurrent()
    {
        audioSource.clip = clips[index];
        audioSource.Play();

        subtitleManager?.OnNewClipStarted();
    }

    public bool IsPlaying => isPlaying;
}
