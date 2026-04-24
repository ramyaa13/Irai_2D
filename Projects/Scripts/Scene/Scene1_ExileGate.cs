using UnityEngine;

public class Scene1_ExileGate : MonoBehaviour
{
    public AudioClip villageMusic;
    public DialogueNode elderOpeningNode;

    void Start()
    {
        AudioManager.Instance?.PlayMusic(villageMusic);
        // Trigger opening dialogue after short delay
        Invoke(nameof(PlayOpening), 1.2f);
    }
    void PlayOpening() { DialogueSystem.Instance?.Play(elderOpeningNode); }
}