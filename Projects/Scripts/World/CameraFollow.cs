using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 1, -10);
    public float smooth = 5f;
    public Vector2 minBounds, maxBounds;  // set per scene
    public float shakeTimer;
    public float shakeMagnitude = 0.1f;

    void LateUpdate()
    {
        if (!target) return;
        Vector3 desired = target.position + offset;
        desired.x = Mathf.Clamp(desired.x, minBounds.x, maxBounds.x);
        desired.y = Mathf.Clamp(desired.y, minBounds.y, maxBounds.y);
        transform.position = Vector3.Lerp(transform.position, desired, smooth * Time.deltaTime);

        if (shakeTimer > 0)
        {
            transform.position += (Vector3)Random.insideUnitCircle * shakeMagnitude;
            shakeTimer -= Time.deltaTime;
        }
    }

    public void Shake(float duration, float mag = 0.15f) { shakeTimer = duration; shakeMagnitude = mag; }
}