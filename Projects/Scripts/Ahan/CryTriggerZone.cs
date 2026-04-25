using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CryTriggerZone : MonoBehaviour
{
    public float cryAddPerSec = 0.2f;
    public float fearAddPerSec = 1.5f;
    public bool oneShot = false;
    bool fired;

    void OnTriggerStay2D(Collider2D other)
    {
        if (oneShot && fired) return;
        if (!other.CompareTag("Player")) return;
        var ahan = other.GetComponentInChildren<AhanSystem>();
        var stats = other.GetComponent<PlayerStats>();
        if (ahan) ahan.cry = Mathf.Min(1f, ahan.cry + cryAddPerSec * Time.deltaTime);
        if (stats) stats.AddFear(fearAddPerSec * Time.deltaTime);
        fired = true;
    }
}