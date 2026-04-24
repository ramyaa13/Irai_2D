using UnityEngine;

public static class PlayerInput
{
    // Touch input is written into these by MobileControls.cs
    public static float touchHoriz = 0f;
    public static bool touchJumpPressed;
    public static bool touchJumpHeld;
    public static bool touchInteractPressed;
    public static bool touchFeedPressed;

    public static float HorizontalAxis
    {
        get
        {
            float k = Input.GetAxisRaw("Horizontal");
            return Mathf.Abs(k) > 0.05f ? k : touchHoriz;
        }
    }

    public static bool JumpPressed =>
        Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) ||
        Input.GetButtonDown("Jump") || ConsumeTouchJump();

    public static bool JumpReleased =>
        Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.W) ||
        Input.GetButtonUp("Jump") || (touchJumpHeld == false && _wasTouchJumpHeld);

    public static bool InteractPressed =>
        Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown(KeyCode.Return) || ConsumeTouchInteract();

    public static bool FeedPressed =>
        Input.GetKeyDown(KeyCode.F) || ConsumeTouchFeed();

    public static bool PausePressed =>
        Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P);

    static bool _wasTouchJumpHeld;
    static bool ConsumeTouchJump() { bool v = touchJumpPressed; touchJumpPressed = false; return v; }
    static bool ConsumeTouchInteract() { bool v = touchInteractPressed; touchInteractPressed = false; return v; }
    static bool ConsumeTouchFeed() { bool v = touchFeedPressed; touchFeedPressed = false; return v; }
}