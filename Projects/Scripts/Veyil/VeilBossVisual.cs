using UnityEngine;

public class VeilBossVisual : MonoBehaviour
{
    public float pulseAmplitude = 0.05f;
    public float pulseSpeed = 1.5f;
    public float driftX = 0.15f;
    public float driftSpeed = 0.6f;
    public Transform target;     // Player

    Vector3 startScale;
    Vector3 startPos;

    void Start()
    {
        startScale = transform.localScale;
        startPos = transform.position;
        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
        }
    }

    void Update()
    {
        // Breathing pulse
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmplitude;
        transform.localScale = startScale * pulse;

        // Subtle drift
        float dx = Mathf.Sin(Time.time * driftSpeed) * driftX;
        transform.position = new Vector3(startPos.x + dx, startPos.y, startPos.z);

        // Always face Irai (small head tilt — flips sprite based on which side player is)
        if (target && target.position.x < transform.position.x)
        {
            var s = transform.localScale; s.x = -Mathf.Abs(s.x); transform.localScale = s;
        }
    }
}