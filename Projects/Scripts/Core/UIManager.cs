using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Image fader;
    public CanvasGroup toastGroup;
    public TMP_Text toastText;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this; DontDestroyOnLoad(gameObject);
    }

    public void Toast(string msg, float duration = 2f) => StartCoroutine(ToastCR(msg, duration));

    IEnumerator ToastCR(string msg, float duration)
    {
        toastText.text = msg;
        float t = 0;
        while (t < 0.25f) { t += Time.unscaledDeltaTime; toastGroup.alpha = t / 0.25f; yield return null; }
        toastGroup.alpha = 1f;
        yield return new WaitForSecondsRealtime(duration);
        t = 0;
        while (t < 0.35f) { t += Time.unscaledDeltaTime; toastGroup.alpha = 1 - t / 0.35f; yield return null; }
        toastGroup.alpha = 0f;
    }
}