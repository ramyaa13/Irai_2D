using UnityEngine;

public class ShadowEnemy : MonoBehaviour
{
    public float speed = 1.2f;
    public float damage = 10f;
    public float attackCooldown = 1.2f;
    public Transform target;          // player
    public Transform firePoint;       // spawned enemies avoid if near fire
    public float fireRepelRadius = 3.5f;
    public SpriteRenderer sr;

    float cooldown;

    void Start()
    {
        if (!target) { var p = GameObject.FindGameObjectWithTag("Player"); if (p) target = p.transform; }
        if (!firePoint) { var f = GameObject.FindGameObjectWithTag("Campfire"); if (f) firePoint = f.transform; }
    }

    void Update()
    {
        if (!target) return;
        Vector2 pos = transform.position;
        Vector2 goal = target.position;

        // Retreat from fire
        if (firePoint && Vector2.Distance(pos, firePoint.position) < fireRepelRadius)
        {
            Vector2 away = (pos - (Vector2)firePoint.position).normalized;
            pos += away * speed * 1.5f * Time.deltaTime;
            if (sr) sr.color = new Color(1, 1, 1, 0.4f);
        }
        else
        {
            pos = Vector2.MoveTowards(pos, goal, speed * Time.deltaTime);
            if (sr) sr.color = Color.white;
        }
        transform.position = pos;

        cooldown -= Time.deltaTime;
        if (cooldown <= 0 && Vector2.Distance(transform.position, target.position) < 0.8f)
        {
            target.GetComponent<PlayerStats>()?.Damage(damage);
            cooldown = attackCooldown;
        }
    }

    public void FadeAndDie() => Destroy(gameObject);
}