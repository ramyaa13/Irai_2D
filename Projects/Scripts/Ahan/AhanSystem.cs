using UnityEngine;

public class AhanSystem : MonoBehaviour
{
    [Header("Cry")]
    public float cry = 0f;            // 0..1, 1 = screaming
    public float cryBuildPerSec = 0.015f;   // baseline
    public float cryFromFearMult = 0.05f;   // fear makes him cry faster
    public float calmPerAction = 0.35f;     // lullaby tap
    public float dangerThreshold = 0.7f;    // >= this alerts enemies

    [Header("Glow (safe indicator)")]
    public SpriteRenderer glow;             // optional child sprite with additive shader
    public float glowMaxAlpha = 0.8f;

    public PlayerStats stats;

    void Start()
    {
        if (!stats) stats = GetComponentInParent<PlayerStats>();
    }

    void Update()
    {
        if (GameManager.Instance && GameManager.Instance.IsPaused) return;
        float fear01 = stats ? stats.fear / stats.maxFear : 0f;
        cry += (cryBuildPerSec + fear01 * cryFromFearMult) * Time.deltaTime;
        cry = Mathf.Clamp01(cry);

        GameEvents.RaiseAhanCry(cry);

        if (glow)
        {
            float a = Mathf.Lerp(glowMaxAlpha, 0f, cry);
            var c = glow.color; c.a = a; glow.color = c;
        }

        if (cry >= dangerThreshold) GameEvents.RaiseAhanInDanger();
    }

    public void Calm(float amount = -1f)
    {
        cry = Mathf.Max(0, cry - (amount < 0 ? calmPerAction : amount));
        if (cry <= 0.05f) GameEvents.RaiseAhanCalmed();
    }
}