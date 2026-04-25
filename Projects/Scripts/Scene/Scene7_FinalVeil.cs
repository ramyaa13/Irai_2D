using UnityEngine;

public class Scene7_FinalVeil : MonoBehaviour
{
    public AudioClip veilMusic;
    void Start()
    {
        AudioManager.Instance?.PlayMusic(veilMusic, 0.5f);

        var pc = FindFirstObjectByType<PlayerController>();
        if (pc) pc.enabled = false;

        // Hide HUD bars - they're meaningless in Scene 7
        var hud = GameObject.Find("Canvas_HUD");
        if (hud) hud.SetActive(false);

        // Start the veil dialogue sequence
        var boss = FindFirstObjectByType<VeilBoss>();
        if (boss) boss.BeginEncounter();
    }
}