using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MobileControls : MonoBehaviour
{
    public GameObject root;
    public RectTransform joystickBg, joystickKnob;
    public float joystickRadius = 80f;
    int joystickTouchId = -1;
    Vector2 joystickCenter;

    void Start()
    {
        bool mobile =
#if UNITY_ANDROID || UNITY_IOS
            true;
#else
            false;
#endif
        root.SetActive(mobile);
    }

    void Update()
    {
        if (!root.activeSelf) return;
        HandleJoystick();
    }

    void HandleJoystick()
    {
        if (joystickTouchId == -1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                if (t.phase == TouchPhase.Began && RectTransformUtility.RectangleContainsScreenPoint(joystickBg, t.position))
                {
                    joystickTouchId = t.fingerId;
                    joystickCenter = t.position;
                    joystickBg.position = t.position;
                    break;
                }
            }
        }
        else
        {
            bool found = false;
            for (int i = 0; i < Input.touchCount; i++)
            {
                var t = Input.GetTouch(i);
                if (t.fingerId != joystickTouchId) continue;
                found = true;
                Vector2 delta = t.position - joystickCenter;
                Vector2 clamped = Vector2.ClampMagnitude(delta, joystickRadius);
                joystickKnob.position = joystickCenter + clamped;
                PlayerInput.touchHoriz = clamped.x / joystickRadius;
                if (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) { Reset(); }
                break;
            }
            if (!found) Reset();
        }
    }

    void Reset()
    {
        joystickTouchId = -1;
        PlayerInput.touchHoriz = 0f;
        joystickKnob.position = joystickBg.position;
    }

    // hook these to UI Button Event Trigger Pointer Down / Up
    public void Btn_JumpDown() { PlayerInput.touchJumpPressed = true; PlayerInput.touchJumpHeld = true; }
    public void Btn_JumpUp() { PlayerInput.touchJumpHeld = false; }
    public void Btn_Interact() { PlayerInput.touchInteractPressed = true; }
    public void Btn_Feed() { PlayerInput.touchFeedPressed = true; }
}