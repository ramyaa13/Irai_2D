using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MusicNote : MonoBehaviour, IPointerClickHandler
{
    public RectTransform rect;
    public Image image;
    public float fallSpeed = 100f;        // px per second
    public float hitWindowPx = 50f;       // distance from sing line to count as hit

    public RectTransform singLine;        // assigned by manager
    public System.Action<MusicNote> onHit;
    public System.Action<MusicNote> onMiss;

    bool resolved;

    void Awake()
    {
        if (!rect) rect = GetComponent<RectTransform>();
        if (!image) image = GetComponent<Image>();
    }

    void Update()
    {
        if (resolved) return;
        rect.anchoredPosition += Vector2.down * fallSpeed * Time.unscaledDeltaTime;

        // Auto-miss if fallen too far past the line
        if (singLine && rect.anchoredPosition.y < singLine.anchoredPosition.y - hitWindowPx * 2.5f)
        {
            resolved = true;
            onMiss?.Invoke(this);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (resolved) return;
        resolved = true;

        float distFromLine = singLine ? Mathf.Abs(rect.anchoredPosition.y - singLine.anchoredPosition.y) : 999f;

        if (distFromLine <= hitWindowPx)
            onHit?.Invoke(this);
        else
            onMiss?.Invoke(this);
    }

    public void FlashAndDie(Color flash)
    {
        if (image) image.color = flash;
        Destroy(gameObject, 0.25f);
    }
}