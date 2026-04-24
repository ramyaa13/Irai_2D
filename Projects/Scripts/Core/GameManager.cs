using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum EndingType { None, Deny, Submit, Accept }

    [Header("Global Run State")]
    public int totalScore;
    public int karma;                  // -100..+100 shapes Veil difficulty + ending gates
    public int memoryShards;
    public int collectiblesFound;
    public int deaths;
    public EndingType ending = EndingType.None;

    [Header("Settings")]
    public float masterVolume = 1f;
    public float musicVolume = 0.8f;
    public float sfxVolume = 1f;
    public bool subtitlesOn = true;

    public bool IsPaused { get; private set; }

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;
    }

    void OnEnable()
    {
        GameEvents.OnScoreDelta += HandleScore;
        GameEvents.OnCollectiblePickup += HandleCollectible;
        GameEvents.OnPlayerDied += HandleDeath;
    }
    void OnDisable()
    {
        GameEvents.OnScoreDelta -= HandleScore;
        GameEvents.OnCollectiblePickup -= HandleCollectible;
        GameEvents.OnPlayerDied -= HandleDeath;
    }

    void HandleScore(int d) { totalScore += d; karma = Mathf.Clamp(karma + d / 10, -100, 100); }
    void HandleCollectible(CollectibleData c) { collectiblesFound++; if (c.isMemoryShard) memoryShards++; }
    void HandleDeath() { deaths++; }

    public void TogglePause() { IsPaused = !IsPaused; Time.timeScale = IsPaused ? 0f : 1f; }

    public void ResetRun()
    {
        totalScore = 0; karma = 0; memoryShards = 0; collectiblesFound = 0; deaths = 0;
        ending = EndingType.None;
    }
}