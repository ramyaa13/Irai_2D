using UnityEngine;

public class FearSystem : MonoBehaviour
{
    public PlayerStats stats;
    public AhanSystem ahan;

    [Header("Sources")]
    public float fearFromHungerLow = 0.6f;      // per sec when hunger < 30
    public float fearFromHealthLow = 0.8f;      // per sec when health < 30
    public float fearFromAhanCryHigh = 1.2f;    // per sec when cry > 0.7
    public float courageDecayIdle = 0.1f;

    void Start()
    {
        if (!stats) stats = GetComponent<PlayerStats>();
        if (!ahan) ahan = GetComponentInChildren<AhanSystem>();
    }

    void Update()
    {
        if (GameManager.Instance && GameManager.Instance.IsPaused) return;
        if (stats.IsDead) return;

        if (stats.hunger < 30f) stats.AddFear(fearFromHungerLow * Time.deltaTime);
        if (stats.health < 30f) stats.AddFear(fearFromHealthLow * Time.deltaTime);
        if (ahan && ahan.cry > 0.7f) stats.AddFear(fearFromAhanCryHigh * Time.deltaTime);

        stats.courage = Mathf.Max(0, stats.courage - courageDecayIdle * Time.deltaTime);
    }
}