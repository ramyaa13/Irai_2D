using System;

public static class GameEvents
{
    // Player
    public static event Action<float, float> OnHealthChanged;
    public static event Action<float, float> OnStaminaChanged;
    public static event Action<float, float> OnHungerChanged;
    public static event Action<float, float> OnFearChanged;
    public static event Action<float, float> OnCourageChanged;
    public static event Action OnPlayerDied;

    // Ahan
    public static event Action<float> OnAhanCryChanged;   // 0..1
    public static event Action OnAhanCalmed;
    public static event Action OnAhanInDanger;

    // Flow
    public static event Action<DialogueNode> OnDialogueStart;
    public static event Action OnDialogueEnd;
    public static event Action<int> OnChoiceMade;         // index
    public static event Action<string> OnCheckpointReached;
    public static event Action<int> OnScoreDelta;
    public static event Action<string> OnSceneCompleted;

    // World
    public static event Action<CollectibleData> OnCollectiblePickup;
    public static event Action<string> OnVeilWhisper;
    public static event Action<float> OnVeilProximity;    // 0..1

    public static void RaiseHealth(float c, float m) => OnHealthChanged?.Invoke(c, m);
    public static void RaiseStamina(float c, float m) => OnStaminaChanged?.Invoke(c, m);
    public static void RaiseHunger(float c, float m) => OnHungerChanged?.Invoke(c, m);
    public static void RaiseFear(float c, float m) => OnFearChanged?.Invoke(c, m);
    public static void RaiseCourage(float c, float m) => OnCourageChanged?.Invoke(c, m);
    public static void RaisePlayerDied() => OnPlayerDied?.Invoke();
    public static void RaiseAhanCry(float v) => OnAhanCryChanged?.Invoke(v);
    public static void RaiseAhanCalmed() => OnAhanCalmed?.Invoke();
    public static void RaiseAhanInDanger() => OnAhanInDanger?.Invoke();
    public static void RaiseDialogueStart(DialogueNode n) => OnDialogueStart?.Invoke(n);
    public static void RaiseDialogueEnd() => OnDialogueEnd?.Invoke();
    public static void RaiseChoiceMade(int i) => OnChoiceMade?.Invoke(i);
    public static void RaiseCheckpoint(string id) => OnCheckpointReached?.Invoke(id);
    public static void RaiseScoreDelta(int s) => OnScoreDelta?.Invoke(s);
    public static void RaiseSceneCompleted(string s) => OnSceneCompleted?.Invoke(s);
    public static void RaiseCollectible(CollectibleData d) => OnCollectiblePickup?.Invoke(d);
    public static void RaiseVeilWhisper(string t) => OnVeilWhisper?.Invoke(t);
    public static void RaiseVeilProximity(float v) => OnVeilProximity?.Invoke(v);

    public static void ClearAll()
    {
        OnHealthChanged = null; OnStaminaChanged = null; OnHungerChanged = null;
        OnFearChanged = null; OnCourageChanged = null; OnPlayerDied = null;
        OnAhanCryChanged = null; OnAhanCalmed = null; OnAhanInDanger = null;
        OnDialogueStart = null; OnDialogueEnd = null; OnChoiceMade = null;
        OnCheckpointReached = null; OnScoreDelta = null; OnSceneCompleted = null;
        OnCollectiblePickup = null; OnVeilWhisper = null; OnVeilProximity = null;
    }
}