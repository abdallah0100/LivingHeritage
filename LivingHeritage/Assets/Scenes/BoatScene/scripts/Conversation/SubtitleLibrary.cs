using System.Collections.Generic;
using UnityEngine;

public class SubtitleLibrary : MonoBehaviour
{
    public List<AudioSubtitleLink> entries = new();

    private Dictionary<AudioClip, TextAsset> map;

    void Awake()
    {
        map = new Dictionary<AudioClip, TextAsset>();

        foreach (var e in entries)
        {
            if (e.audioClip != null && e.subtitleJson != null && !map.ContainsKey(e.audioClip))
                map.Add(e.audioClip, e.subtitleJson);
        }
    }

    public TextAsset GetJson(AudioClip clip)
    {
        if (clip == null) return null;
        map.TryGetValue(clip, out var json);
        return json;
    }
}
