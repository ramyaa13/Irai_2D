using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LullabyMiniGame : MonoBehaviour
{
    public static bool IsActive { get; private set; }

    [Header("UI")]
    public CanvasGroup panelGroup;
    public RectTransform spawnArea;       // NoteSpawnArea
    public RectTransform singLine;        // SingLine
    public MusicNote notePrefab;
    public TMP_Text progressText;
    public TMP_Text titleText;
    public TMP_Text resultText;

    [Header("Gameplay")]
    public int notesToCalm = 3;
    public float spawnInterval = 1.4f;
    public float fallSpeed = 180f;
    public float hitWindowPx = 60f;
    public float fadeIn = 0.25f;
    public float introHold = 0.5f;
    public float endHold = 1.4f;

    [Header("Colors")]
    public Color hitFlash = new Color(0.5f, 1f, 0.6f, 1f);
    public Color missFlash = new Color(1f, 0.4f, 0.4f, 1f);

    AhanSystem ahan;
    int hits;
    int missesAndExpired;
    int notesSpawned;
    bool gameOver;

    public void Begin(AhanSystem a)
    {
        if (IsActive) return;
        ahan = a;
        StartCoroutine(BeginCR());
    }

    IEnumerator BeginCR()
    {
        IsActive = true;
        gameOver = false;
        hits = 0;
        missesAndExpired = 0;
        notesSpawned = 0;
        if (resultText) resultText.text = "";
        if (titleText) titleText.text = "Sing to Ahan";
        UpdateProgress();

        // Fade in
        if (panelGroup)
        {
            panelGroup.blocksRaycasts = true;
            float t = 0;
            while (t < fadeIn) { t += Time.unscaledDeltaTime; panelGroup.alpha = t / fadeIn; yield return null; }
            panelGroup.alpha = 1;
        }

        yield return new WaitForSecondsRealtime(introHold);

        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (!gameOver && notesSpawned < notesToCalm + 2)  // spawn a couple extra for forgiveness
        {
            SpawnNote();
            yield return new WaitForSecondsRealtime(spawnInterval);
        }
        // If still not finished by spawn time, wait a bit more for last notes to fall
        yield return new WaitForSecondsRealtime(2.5f);
        if (!gameOver) yield return Finish(hits >= notesToCalm);
    }

    void SpawnNote()
    {
        if (!notePrefab || !spawnArea) return;
        var note = Instantiate(notePrefab, spawnArea);

        // Random horizontal position within spawn area
        float halfWidth = spawnArea.rect.width / 2f - 50f;
        float x = Random.Range(-halfWidth, halfWidth);
        float topY = spawnArea.rect.height / 2f;
        note.rect.anchoredPosition = new Vector2(x, topY);

        note.singLine = singLine;
        note.fallSpeed = fallSpeed;
        note.hitWindowPx = hitWindowPx;
        note.onHit = OnNoteHit;
        note.onMiss = OnNoteMiss;

        notesSpawned++;
    }

    void OnNoteHit(MusicNote n)
    {
        hits++;
        n.FlashAndDie(hitFlash);
        if (ahan) ahan.Calm(0.4f);
        UpdateProgress();
        AudioManager.Instance?.PlaySFX(AudioManager.Instance.sfxAhanCalm);

        if (hits >= notesToCalm && !gameOver)
        {
            gameOver = true;
            StartCoroutine(Finish(true));
        }
    }

    void OnNoteMiss(MusicNote n)
    {
        missesAndExpired++;
        n.FlashAndDie(missFlash);
        if (ahan) ahan.cry = Mathf.Min(1f, ahan.cry + 0.08f);
        if (missesAndExpired >= 3 && !gameOver)
        {
            gameOver = true;
            StartCoroutine(Finish(false));
        }
    }

    void UpdateProgress()
    {
        if (progressText) progressText.text = $"{hits} / {notesToCalm}";
    }

    IEnumerator Finish(bool success)
    {
        gameOver = true;
        if (resultText) resultText.text = success ? "He sleeps." : "He weeps.";

        yield return new WaitForSecondsRealtime(endHold);

        // Clear remaining notes
        foreach (var n in spawnArea.GetComponentsInChildren<MusicNote>())
            if (n) Destroy(n.gameObject);

        if (panelGroup)
        {
            float t = 0;
            while (t < fadeIn) { t += Time.unscaledDeltaTime; panelGroup.alpha = 1 - (t / fadeIn); yield return null; }
            panelGroup.alpha = 0;
            panelGroup.blocksRaycasts = false;
        }

        if (success)
        {
            GameEvents.RaiseAhanCalmed();
            GameEvents.RaiseScoreDelta(50);
        }

        IsActive = false;
    }
}