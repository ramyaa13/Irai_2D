using UnityEngine;

public class Scene7_FinalVeil : MonoBehaviour
{
    public AudioClip veilMusic;
    void Start()
    {
        AudioManager.Instance?.PlayMusic(veilMusic);
        // Disable normal movement — pure dialogue scene
        var pc = FindObjectOfType<PlayerController>();
        if (pc) pc.enabled = false;
    }
}