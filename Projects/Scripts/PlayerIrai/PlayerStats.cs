using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Header("Health")]
    public float maxHealth = 100f;
    public float health;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float stamina;
    public float staminaRegen = 15f;
    public float minStaminaForJump = 10f;

    [Header("Hunger")]
    public float maxHunger = 100f;
    public float hunger;
    public float hungerDrainPerSec = 0.8f;
    public float starvationDamagePerSec = 2f;

    [Header("Fear / Courage")]
    public float maxFear = 100f;
    public float fear;
    public float maxCourage = 100f;
    public float courage = 30f;

    [Header("Invulnerability")]
    public float invulnTime = 0.6f;
    float invulnTimer;

    public bool IsDead => health <= 0f;
    public bool IsInvuln => invulnTimer > 0f;

    void Start()
    {
        health = maxHealth; stamina = maxStamina; hunger = maxHunger; fear = 0f;
        PushAllToUI();
    }

    void Update()
    {
        if (GameManager.Instance && GameManager.Instance.IsPaused) return;
        if (IsDead) return;

        // Hunger tick
        hunger = Mathf.Max(0, hunger - hungerDrainPerSec * Time.deltaTime);
        if (hunger <= 0) Damage(starvationDamagePerSec * Time.deltaTime, silent: true);

        // Stamina regen
        if (stamina < maxStamina) stamina = Mathf.Min(maxStamina, stamina + staminaRegen * Time.deltaTime);

        if (invulnTimer > 0) invulnTimer -= Time.deltaTime;

        GameEvents.RaiseHealth(health, maxHealth);
        GameEvents.RaiseStamina(stamina, maxStamina);
        GameEvents.RaiseHunger(hunger, maxHunger);
        GameEvents.RaiseFear(fear, maxFear);
        GameEvents.RaiseCourage(courage, maxCourage);
    }

    public bool CanJump() => stamina >= minStaminaForJump;
    public void DrainStamina(float amt) { stamina = Mathf.Max(0, stamina - amt); }

    public void Damage(float amt, bool silent = false)
    {
        if (IsInvuln || IsDead) return;
        health = Mathf.Max(0, health - amt);
        if (!silent) invulnTimer = invulnTime;
        if (health <= 0) GameEvents.RaisePlayerDied();
    }

    public void Heal(float amt) { health = Mathf.Min(maxHealth, health + amt); }
    public void Eat(float amt) { hunger = Mathf.Min(maxHunger, hunger + amt); }

    public void AddFear(float amt) { fear = Mathf.Clamp(fear + amt, 0, maxFear); }
    public void AddCourage(float amt) { courage = Mathf.Clamp(courage + amt, 0, maxCourage); fear = Mathf.Max(0, fear - amt * 0.5f); }

    void PushAllToUI()
    {
        GameEvents.RaiseHealth(health, maxHealth);
        GameEvents.RaiseStamina(stamina, maxStamina);
        GameEvents.RaiseHunger(hunger, maxHunger);
        GameEvents.RaiseFear(fear, maxFear);
        GameEvents.RaiseCourage(courage, maxCourage);
    }
}