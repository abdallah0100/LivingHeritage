using UnityEngine;

public class SymbolAudio : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip voiceClip;

    private bool hasPlayed = false; //  play-once flag

    public void PlaySymbolSound()
    {
        //  Already played do nothing
        if (hasPlayed)
            return;

        if (audioSource != null && voiceClip != null)
        {
            audioSource.clip = voiceClip;
            audioSource.Play();
            hasPlayed = true; //  mark as played
        }
    }

    public void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
