using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance;
    public Image fader;
    public float fadeTime = 0.6f;

    void Awake()
    {
        if (Instance && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Load(string sceneName) => StartCoroutine(LoadCR(sceneName));

    IEnumerator LoadCR(string sceneName)
    {
        // If fader isn't already black, fade to black first
        if (fader && fader.color.a < 0.99f)
            yield return Fade(fader.color.a, 1f);
        else
            SetAlpha(1f);

        GameEvents.ClearAll();
        var op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        // Fade out of black
        yield return Fade(1f, 0f);
    }

    IEnumerator Fade(float from, float to)
    {
        if (!fader) yield break;
        float t = 0f;
        while (t < fadeTime)
        {
            t += Time.unscaledDeltaTime;
            SetAlpha(Mathf.Lerp(from, to, t / fadeTime));
            yield return null;
        }
        SetAlpha(to);
    }

    void SetAlpha(float a)
    {
        if (!fader) return;
        var c = fader.color; c.a = a; fader.color = c;
    }
}