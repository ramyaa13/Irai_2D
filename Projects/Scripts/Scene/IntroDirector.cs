using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class IntroDirector : MonoBehaviour
{
    [Serializable]
    public class Slide
    {
        public Image image;                 // full-screen Image for this slide
        [TextArea(2, 4)] public string text; // narration line
        public float holdSeconds = 4f;      // how long text stays after typing completes
        public AudioClip voice;             // optional narration audio
    }

    [Header("Slides (in order)")]
    public Slide[] slides;

    [Header("Refs")]
    public TMP_Text monologueText;
    public Image fadeToBlack;
    public Button skipButton;
    public CanvasGroup textPanelGroup;      // optional, fades text panel in/out

    [Header("Timing")]
    public float slideFadeTime = 1.0f;
    public float charsPerSecond = 30f;
    public float textFadeTime = 0.5f;

    [Header("Flow")]
    public string nextSceneName = "03_ExileGate";
    public AudioClip introMusic;

    bool skipRequested;

    void Start()
    {
        // Hide all slides except first
        for (int i = 0; i < slides.Length; i++)
            if (slides[i].image) SetAlpha(slides[i].image, i == 0 ? 1f : 0f);

        if (fadeToBlack) SetAlpha(fadeToBlack, 0f);
        if (monologueText) monologueText.text = "";

        if (skipButton) skipButton.onClick.AddListener(RequestSkip);

        if (AudioManager.Instance && introMusic)
            AudioManager.Instance.PlayMusic(introMusic, 0.6f);

        StartCoroutine(Run());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) ||
            Input.GetKeyDown(KeyCode.Return) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            Input.GetMouseButtonDown(0))
        {
            RequestSkip();
        }
    }

    void RequestSkip() { skipRequested = true; }

    IEnumerator Run()
    {
        for (int i = 0; i < slides.Length; i++)
        {
            yield return PlaySlide(i);
            if (skipRequested) break;
        }
        yield return EndSequence();
    }

    IEnumerator PlaySlide(int i)
    {
        var s = slides[i];

        // Crossfade from previous slide (if any)
        if (i > 0 && slides[i - 1].image && s.image)
            yield return CrossFade(slides[i - 1].image, s.image, slideFadeTime);

        if (s.voice && AudioManager.Instance)
            AudioManager.Instance.PlaySFX(s.voice);

        // Type the text
        yield return TypeText(s.text);
        if (skipRequested) yield break;

        // Hold
        float t = 0f;
        while (t < s.holdSeconds && !skipRequested)
        {
            t += Time.deltaTime;
            yield return null;
        }

        // Fade text out before next slide
        if (i < slides.Length - 1)
            yield return FadeText(1f, 0f);
    }

    IEnumerator TypeText(string full)
    {
        monologueText.text = "";
        yield return FadeText(0f, 1f);

        int shown = 0;
        float acc = 0f;
        while (shown < full.Length)
        {
            if (skipRequested) { monologueText.text = full; yield break; }
            acc += Time.deltaTime * charsPerSecond;
            int add = Mathf.FloorToInt(acc);
            if (add > 0)
            {
                shown = Mathf.Min(full.Length, shown + add);
                monologueText.text = full.Substring(0, shown);
                acc -= add;
            }
            yield return null;
        }
    }

    IEnumerator FadeText(float from, float to)
    {
        if (!textPanelGroup) { yield break; }
        float t = 0f;
        while (t < textFadeTime)
        {
            t += Time.deltaTime;
            textPanelGroup.alpha = Mathf.Lerp(from, to, t / textFadeTime);
            yield return null;
        }
        textPanelGroup.alpha = to;
    }

    IEnumerator CrossFade(Image from, Image to, float dur)
    {
        float t = 0f;
        while (t < dur)
        {
            t += Time.deltaTime;
            float k = t / dur;
            SetAlpha(from, 1f - k);
            SetAlpha(to, k);
            yield return null;
        }
        SetAlpha(from, 0f);
        SetAlpha(to, 1f);
    }

    IEnumerator EndSequence()
    {
        // Fade monologue
        yield return FadeText(1f, 0f);

        // Fade whole screen to black
        if (fadeToBlack)
        {
            float t = 0f;
            while (t < 1.2f)
            {
                t += Time.deltaTime;
                SetAlpha(fadeToBlack, Mathf.Clamp01(t / 1.2f));
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f);

        if (SceneLoader.Instance != null)
            SceneLoader.Instance.Load(nextSceneName);
        else
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
    }

    static void SetAlpha(Image img, float a)
    {
        if (!img) return;
        var c = img.color; c.a = a; img.color = c;
    }
}