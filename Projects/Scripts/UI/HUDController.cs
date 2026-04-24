using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDController : MonoBehaviour
{
    public Image healthFill, staminaFill, hungerFill, fearFill, courageFill, ahanCryFill;
    public TMP_Text scoreText;
    public GameObject lowHealthFlash;

    int score;

    void OnEnable()
    {
        GameEvents.OnHealthChanged += (c, m) => Set(healthFill, c, m);
        GameEvents.OnStaminaChanged += (c, m) => Set(staminaFill, c, m);
        GameEvents.OnHungerChanged += (c, m) => Set(hungerFill, c, m);
        GameEvents.OnFearChanged += (c, m) => Set(fearFill, c, m);
        GameEvents.OnCourageChanged += (c, m) => Set(courageFill, c, m);
        GameEvents.OnAhanCryChanged += v => { if (ahanCryFill) ahanCryFill.fillAmount = v; };
        GameEvents.OnScoreDelta += OnScore;
        GameEvents.OnHealthChanged += ToggleLowHealth;
    }
    void OnDisable()
    {
        GameEvents.OnScoreDelta -= OnScore;
    }

    void Set(Image img, float cur, float max)
    {
        if (img) img.fillAmount = Mathf.Clamp01(cur / max);
    }

    void OnScore(int d) { score += d; if (scoreText) scoreText.text = score.ToString(); }

    void ToggleLowHealth(float cur, float max)
    {
        if (!lowHealthFlash) return;
        lowHealthFlash.SetActive(cur / max < 0.25f);
    }
}