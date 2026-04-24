using UnityEngine;

public class Scene2_Desert : MonoBehaviour
{
    public AudioClip desertMusic;
    public LullabyMiniGame lullaby;
    public AhanSystem ahan;

    bool lullabyOfferedRecently;

    void Start()
    {
        AudioManager.Instance?.PlayMusic(desertMusic);
        GameEvents.OnAhanInDanger += TryOfferLullaby;
    }
    void OnDestroy() { GameEvents.OnAhanInDanger -= TryOfferLullaby; }

    void TryOfferLullaby()
    {
        if (lullabyOfferedRecently) return;
        lullabyOfferedRecently = true;
        UIManager.Instance?.Toast("Press E (or Interact) when the dot glows — sing the lullaby.");
        lullaby.Begin(ahan);
        Invoke(nameof(AllowAgain), 10f);
    }
    void AllowAgain() { lullabyOfferedRecently = false; }
}