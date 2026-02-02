using System;
using System.Collections.Generic;

[Serializable]
public class SubtitleTrack
{
    public string audioId;
    public List<SubtitleCue> cues;
}

[Serializable]
public class SubtitleCue
{
    public float start;
    public float end;
    public List<LocalizedLine> lines;
}

[Serializable]
public class LocalizedLine
{
    public string lang; // "en", "he", "ar"
    public string text;
}
