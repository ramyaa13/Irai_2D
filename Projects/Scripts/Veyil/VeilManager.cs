using System.Collections;
using UnityEngine;

public class VeilManager : MonoBehaviour
{
    public static VeilManager Instance;

    [Header("Whispers")]
    public string[] whisperLines;
    public float whisperIntervalMin = 8f;
    public float whisperIntervalMax = 14f;

    [Header("Proximity")]
    public Transform player;
    public float dangerDistance = 3f;    // close = more intense
    public float safeDistance = 15f;

    [Header("Difficulty scaling")]
    public float fearToVeilDamageMult = 0.02f;  // fear%*this = dps when Veil touches
    public float dangerProximity01;             // readable by other systems (vignette)

    public PlayerStats playerStats;

    void Awake() { Instance = this; }

    void Start()
    {
        StartCoroutine(WhisperLoop());
    }

    void Update()
    {
        if (!player || !playerStats) return;

        float dist = Vector2.Distance(transform.position, player.position);
        dangerProximity01 = 1f - Mathf.InverseLerp(dangerDistance, safeDistance, dist);
        dangerProximity01 = Mathf.Clamp01(dangerProximity01);
        GameEvents.RaiseVeilProximity(dangerProximity01);

        // Adaptive: if player is very afraid, Veil drains their stamina just by being near
        if (dangerProximity01 > 0.6f && playerStats.fear > 60f)
            playerStats.DrainStamina(3f * Time.deltaTime);
    }

    IEnumerator WhisperLoop()
    {
        while (true)
        {
            float wait = Random.Range(whisperIntervalMin, whisperIntervalMax);
            yield return new WaitForSeconds(wait);
            if (!playerStats || playerStats.IsDead) continue;
            // More whispers when fear is high
            float fearMod = Mathf.Lerp(0.8f, 0.3f, playerStats.fear / playerStats.maxFear);
            yield return new WaitForSeconds(wait * fearMod);
            if (whisperLines.Length == 0) continue;
            string line = whisperLines[Random.Range(0, whisperLines.Length)];
            GameEvents.RaiseVeilWhisper(line);
            playerStats.AddFear(3f);
        }
    }
}