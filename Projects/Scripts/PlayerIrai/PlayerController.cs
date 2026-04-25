using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 4.5f;
    public float jumpForce = 11f;
    public float coyoteTime = 0.12f;
    public float jumpBuffer = 0.1f;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundRadius = 0.15f;
    public LayerMask groundMask;

    [Header("Refs")]
    public PlayerStats stats;
    public PlayerAnimator anim;

    // External modifiers - set by WindZone / cold / fear / weight
    [HideInInspector] public float externalForceX = 0f;
    [HideInInspector] public float speedMultiplier = 1f;

    Rigidbody2D rb;
    float coyote, buffer, input;
    bool facingRight = true, grounded;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!stats) stats = GetComponent<PlayerStats>();
        if (!anim) anim = GetComponent<PlayerAnimator>();
        rb.freezeRotation = true;
    }

    void Update()
    {

        if (GameManager.Instance && GameManager.Instance.IsPaused) return;
        if (stats && stats.IsDead) { rb.linearVelocity = Vector2.zero; return; }


        if (dialogueLock || LullabyMiniGame.IsActive)                        // <-- add these 4 lines
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            anim?.UpdateLocomotion(0, true, 0);
            return;
        }

        input = PlayerInput.HorizontalAxis;
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, groundMask);

        coyote = grounded ? coyoteTime : coyote - Time.deltaTime;
        buffer -= Time.deltaTime;
        if (PlayerInput.JumpPressed) buffer = jumpBuffer;

        if (buffer > 0 && coyote > 0 && stats.CanJump())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            stats.DrainStamina(8f);
            coyote = 0; buffer = 0;
            anim?.PlayJump();
        }

        if (PlayerInput.JumpReleased && rb.linearVelocity.y > 0)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.5f);

        if (input > 0.05f && !facingRight) Flip();
        else if (input < -0.05f && facingRight) Flip();

        anim?.UpdateLocomotion(Mathf.Abs(input) * speedMultiplier, grounded, rb.linearVelocity.y);
    }

    void FixedUpdate()
    {
        if (dialogueLock) 
        { 
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
            return; 
        }
        float vx = input * moveSpeed * speedMultiplier + externalForceX;
        rb.linearVelocity = new Vector2(vx, rb.linearVelocity.y);
    }

    void Flip()
    {
        facingRight = !facingRight;
        var s = transform.localScale; s.x *= -1; transform.localScale = s;
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheck) return;
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    bool dialogueLock = false;

    void OnEnable()
    {
        GameEvents.OnDialogueStart += OnDialogueStart;
        GameEvents.OnDialogueEnd += OnDialogueEnd;
    }

    void OnDisable()
    {
        GameEvents.OnDialogueStart -= OnDialogueStart;
        GameEvents.OnDialogueEnd -= OnDialogueEnd;
    }

    void OnDialogueStart(DialogueNode _) { dialogueLock = true; }
    void OnDialogueEnd() { dialogueLock = false; }
}