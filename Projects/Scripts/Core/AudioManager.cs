using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;
    public AudioSource ambienceSource;
    public AudioSource whisperSource;

    [Header("Music")]
    public AudioClip menuMusic, villageMusic, desertMusic, campMusic, veilMusic, endingCalm, endingDark;

    [Header("SFX")]
    public AudioClip sfxPickup, sfxJump, sfxHurt, sfxAhanCry, sfxAhanCalm, sfxWhisper, sfxChoice;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
    }

    void OnEnable()
    {
        GameEvents.OnCollectiblePickup += _ => PlaySFX(sfxPickup);
        GameEvents.OnVeilWhisper += _ => PlaySFX(sfxWhisper);
        GameEvents.OnAhanCalmed += () => PlaySFX(sfxAhanCalm);
    }

    public void PlayMusic(AudioClip clip, float vol = 0.8f)
    {
        if (!musicSource || clip == null || musicSource.clip == clip) return;
        musicSource.clip = clip; musicSource.loop = true; musicSource.volume = vol; musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float vol = 1f)
    {
        if (sfxSource && clip) sfxSource.PlayOneShot(clip, vol);
    }

    public void PlayAmbience(AudioClip clip, float vol = 0.5f)
    {
        if (!ambienceSource || clip == null) return;
        ambienceSource.clip = clip; ambienceSource.loop = true; ambienceSource.volume = vol; ambienceSource.Play();
    }
}