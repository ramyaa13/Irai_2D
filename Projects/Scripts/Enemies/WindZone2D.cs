using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WindZone2D : MonoBehaviour
{
    public float forceX = -3.5f;      // negative = pushes left
    public float staminaDrainPerSec = 5f;

    void OnTriggerStay2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var pc = other.GetComponent<PlayerController>();
        var ps = other.GetComponent<PlayerStats>();
        if (pc) pc.externalForceX = forceX;
        if (ps) ps.DrainStamina(staminaDrainPerSec * Time.deltaTime);
    }
    void OnTriggerExit2D(Collider2D other)
    {
        var pc = other.GetComponent<PlayerController>();
        if (pc) pc.externalForceX = 0f;
    }
}