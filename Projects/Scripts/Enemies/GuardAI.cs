using UnityEngine;

public class GuardAI : MonoBehaviour
{
    public enum State { Patrol, Confront, Hostile, Pacified }

    [Header("Patrol")]
    public Transform pointA, pointB;
    public float speed = 1.5f;

    [Header("Detection")]
    public Transform visionOrigin;
    public float visionRange = 4f;
    public LayerMask playerMask;
    public float ahanHeardRange = 6f;

    [Header("Confront")]
    public DialogueNode confrontNode;       // <-- assign Node_Guard_Confront
    public float confrontDistance = 2.0f;   // closer than vision = forced dialogue
    public bool oneShotDialogue = true;

    [Header("Hostile")]
    public float hostileChaseSpeedMult = 1.6f;
    public float hostileDamage = 8f;
    public float hostileAttackRange = 0.8f;
    public float hostileAttackCooldown = 0.6f;

    [Header("Pacified")]
    public Color pacifiedTint = new Color(0.6f, 1f, 0.6f);

    State state = State.Patrol;
    Vector2 patrolTarget;
    Transform player;
    PlayerStats cachedPlayerStats;
    SpriteRenderer sr;
    bool dialogueFired;
    float attackTimer;

    void Start()
    {

        if (pointA) patrolTarget = pointA.position;
        sr = GetComponent<SpriteRenderer>();
        GameEvents.OnAhanInDanger += OnAhanCryHeard;
        GameEvents.OnGuardOutcome += OnGuardOutcome;

        // Restore pacified state from previous attempt
        if (PlayerPrefs.GetInt("guard_pacified_" + name, 0) == 1)
        {
            state = State.Pacified;
            dialogueFired = true;
            if (sr) sr.color = pacifiedTint;
            transform.position += Vector3.left * 1.5f;
        }
    }

    void OnDestroy()
    {
        GameEvents.OnAhanInDanger -= OnAhanCryHeard;
        GameEvents.OnGuardOutcome -= OnGuardOutcome;
    }

    void Update()
    {
        switch (state)
        {
            case State.Patrol: UpdatePatrol(); break;
            case State.Confront: UpdateConfront(); break;
            case State.Hostile: UpdateHostile(); break;
            case State.Pacified: UpdatePatrol(); break;   // pacified guards still walk their route, just don't react
        }
    }

    // -------- PATROL --------
    void UpdatePatrol()
    {
        if (!pointA || !pointB) return;

        Vector2 p = transform.position;
        p = Vector2.MoveTowards(p, patrolTarget, speed * Time.deltaTime);
        transform.position = p;

        if (Vector2.Distance(p, patrolTarget) < 0.1f)
        {
            patrolTarget = Vector2.Distance(patrolTarget, (Vector2)pointA.position) < 0.1f
                ? (Vector2)pointB.position
                : (Vector2)pointA.position;
        }

        FaceTowards(patrolTarget.x);

        if (state == State.Pacified) return;   // pacified guards ignore detection

        // Detection check
        var hit = visionOrigin
            ? Physics2D.OverlapCircle(visionOrigin.position, visionRange, playerMask)
            : null;
        if (hit)
        {
            player = hit.transform;
            cachedPlayerStats = hit.GetComponent<PlayerStats>();

            // If close enough to confront, switch to dialogue. Otherwise just track.
            if (Vector2.Distance(transform.position, player.position) < visionRange)
                EnterConfront();
        }
    }

    // -------- CONFRONT --------
    void EnterConfront()
    {
        if (oneShotDialogue && dialogueFired) { EnterHostile(); return; }
        if (!confrontNode || DialogueSystem.Instance == null) { EnterHostile(); return; }

        state = State.Confront;
        dialogueFired = true;
        DialogueSystem.Instance.Play(confrontNode);
    }

    void UpdateConfront()
    {
        // Guard stands still and stares; outcome event will move us out of this state
        if (player) FaceTowards(player.position.x);
    }

    // -------- HOSTILE --------
    void EnterHostile()
    {
        state = State.Hostile;
        if (sr) sr.color = Color.red;
    }

    void UpdateHostile()
    {
        if (!player) { state = State.Patrol; return; }

        Vector2 p = Vector2.MoveTowards(transform.position, player.position, speed * hostileChaseSpeedMult * Time.deltaTime);
        transform.position = p;
        FaceTowards(player.position.x);

        attackTimer -= Time.deltaTime;

    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (state != State.Hostile) return;          // only damage when hostile
        if (!other.CompareTag("Player")) return;
        if (attackTimer > 0) return;

        var ps = other.GetComponent<PlayerStats>();
        if (!ps) return;

        ps.Damage(hostileDamage);
        attackTimer = hostileAttackCooldown;

        // Knockback
        var rb = other.GetComponent<Rigidbody2D>();
        if (rb)
        {
            Vector2 dir = ((Vector2)other.transform.position - (Vector2)transform.position).normalized;
            rb.linearVelocity = new Vector2(dir.x * 6f, 4f);
        }
    }

    // -------- EVENT HANDLERS --------
    void OnGuardOutcome(string outcome)
    {
        switch (outcome)
        {
            case "pass":
                // Guard lets her go this once. Returns to patrol but won't re-confront.
                state = State.Pacified;
                if (sr) sr.color = pacifiedTint;
                PlayerPrefs.SetInt("guard_pacified_" + name, 1);
                StartCoroutine(StepAside());
                break;

            case "fight":
                EnterHostile();
                break;

            case "ignore":
                // Player chose to walk away mid-confront. Guard returns to patrol but will retrigger.
                state = State.Patrol;
                dialogueFired = false;
                break;
        }
    }

    System.Collections.IEnumerator StepAside()
    {
        Vector3 start = transform.position;
        Vector3 target = start + new Vector3(0, 0, 0) + Vector3.left * 1.5f;  // step left 1.5 units
                                                                              // pick direction away from player so she always moves out of the path
        if (player) target = start + (transform.position - player.position).normalized * 2f;

        float t = 0f;
        while (t < 0.6f)
        {
            t += Time.deltaTime;
            transform.position = Vector3.Lerp(start, target, t / 0.6f);
            yield return null;
        }
        transform.position = target;

        // Stop patrol so she stays out of the way
        pointA = null;
        pointB = null;
    }

    void OnAhanCryHeard()
    {
        if (state != State.Patrol) return;
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p && Vector2.Distance(transform.position, p.transform.position) < ahanHeardRange)
        {
            player = p.transform;
            cachedPlayerStats = p.GetComponent<PlayerStats>();
            EnterConfront();
        }
    }

    // -------- HELPERS --------
    void FaceTowards(float worldX)
    {
        var s = transform.localScale;
        float dir = worldX - transform.position.x;
        if (Mathf.Abs(dir) < 0.01f) return;
        s.x = Mathf.Abs(s.x) * Mathf.Sign(dir);
        transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (visionOrigin)
        {
            Gizmos.color = state == State.Hostile ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(visionOrigin.position, visionRange);
        }
    }
}