using UnityEngine;

public class VeilFollower : MonoBehaviour
{
    public Transform target;
    public Vector2 offset = new Vector2(-5, 0);
    public float followSpeed = 1.2f;
    public float minScale = 0.6f;
    public float maxScale = 1.8f;

    public PlayerStats stats;

    void Start()
    {
        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p) target = p.transform;
            if (p) stats = p.GetComponent<PlayerStats>();
        }
    }

    void Update()
    {
        if (!target) return;
        Vector3 desired = (Vector2)target.position + offset;
        desired.z = transform.position.z;
        transform.position = Vector3.Lerp(transform.position, desired, followSpeed * Time.deltaTime);

        // Scale with fear
        if (stats)
        {
            float fear01 = stats.fear / stats.maxFear;
            float scale = Mathf.Lerp(minScale, maxScale, fear01);
            transform.localScale = new Vector3(scale, scale, 1);
        }
    }
}