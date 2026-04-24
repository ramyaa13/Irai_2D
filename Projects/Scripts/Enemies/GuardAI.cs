using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public Transform pointA, pointB;
    public float speed = 1.5f;
    public float visionRange = 4f;
    public LayerMask playerMask;
    public Transform visionOrigin;
    public float ahanHeardRange = 6f;

    Transform target;
    Vector2 patrolTarget;
    bool alerted;
    float alertTimer;

    PlayerStats cachedPlayer;

    void Start()
    {
        patrolTarget = pointA.position;
        GameEvents.OnAhanInDanger += OnAhanCryHeard;
    }
    void OnDestroy() { GameEvents.OnAhanInDanger -= OnAhanCryHeard; }

    void Update()
    {
        if (alerted)
        {
            alertTimer -= Time.deltaTime;
            ChasePlayer();
            if (alertTimer <= 0) alerted = false;
        }
        else
        {
            Patrol();
            ScanForPlayer();
        }
    }

    void Patrol()
    {
        Vector2 p = transform.position;
        p = Vector2.MoveTowards(p, patrolTarget, speed * Time.deltaTime);
        transform.position = p;

        if (Vector2.Distance(p, patrolTarget) < 0.1f)
        {
            patrolTarget = Vector2.Distance(patrolTarget, (Vector2)pointA.position) < 0.1f
                ? (Vector2)pointB.position
                : (Vector2)pointA.position;
        }

        // Face movement direction
        float dirX = patrolTarget.x - p.x;
        if (Mathf.Abs(dirX) > 0.01f)
        {
            var s = transform.localScale;
            s.x = Mathf.Abs(s.x) * Mathf.Sign(dirX);
            transform.localScale = s;
        }
    }

    void ScanForPlayer()
    {
        var hit = Physics2D.OverlapCircle(visionOrigin.position, visionRange, playerMask);
        if (hit)
        {
            alerted = true; alertTimer = 3f;
            target = hit.transform;
            cachedPlayer = hit.GetComponent<PlayerStats>();
        }
    }

    void ChasePlayer()
    {
        if (!target) return;
        Vector2 p = Vector2.MoveTowards(transform.position, target.position, speed * 1.6f * Time.deltaTime);
        transform.position = p;
        if (Vector2.Distance(transform.position, target.position) < 0.8f && cachedPlayer)
            cachedPlayer.Damage(5f);
    }

    void OnAhanCryHeard()
    {
        if (!target)
        {
            var p = GameObject.FindGameObjectWithTag("Player");
            if (p && Vector2.Distance(transform.position, p.transform.position) < ahanHeardRange)
            {
                alerted = true; alertTimer = 4f; target = p.transform; cachedPlayer = p.GetComponent<PlayerStats>();
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        if (visionOrigin) { Gizmos.color = Color.yellow; Gizmos.DrawWireSphere(visionOrigin.position, visionRange); }
    }
}