using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collectible : MonoBehaviour
{
    public CollectibleData data;
    public float bobAmp = 0.1f;
    public float bobFreq = 2f;
    Vector3 startPos;

    void Start() { startPos = transform.position; }
    void Update()
    {
        transform.position = startPos + Vector3.up * Mathf.Sin(Time.time * bobFreq) * bobAmp;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var stats = other.GetComponent<PlayerStats>();
        var ahan = other.GetComponentInChildren<AhanSystem>();
        if (stats)
        {
            if (data.healAmount > 0) stats.Heal(data.healAmount);
            if (data.feedAmount > 0) stats.Eat(data.feedAmount);
            if (data.courageBonus > 0) stats.AddCourage(data.courageBonus);
        }
        if (ahan && data.ahanFeedAmount > 0) ahan.Calm(data.ahanFeedAmount);

        GameEvents.RaiseCollectible(data);
        GameEvents.RaiseScoreDelta(data.scoreValue);
        if (data.pickupSfx) AudioManager.Instance?.PlaySFX(data.pickupSfx);

        Destroy(gameObject);
    }
}