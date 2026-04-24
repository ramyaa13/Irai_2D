using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speaker;          // "Irai", "Elder", "Veil", "Ahan"
    [TextArea(2, 5)] public string text;
    public AudioClip voice;
    public float extraDelay = 0f;
}