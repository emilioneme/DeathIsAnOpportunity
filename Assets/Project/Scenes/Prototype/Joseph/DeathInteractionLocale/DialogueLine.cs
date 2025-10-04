using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea] public string text;
    public float typingSpeed = 0.05f;
    public AudioClip voiceClip;
}