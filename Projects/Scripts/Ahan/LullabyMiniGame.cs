using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LullabyMiniGame : MonoBehaviour
{
    public GameObject panel;
    public Image[] beatIndicators;   // 3 dots
    public float beatInterval = 0.9f;
    public float hitWindow = 0.25f;
    public AhanSystem ahan;

    bool active;
    int currentBeat;
    float nextBeatTime;

    public void Begin(AhanSystem a)
    {
        ahan = a;
        gameObject.SetActive(true);
        panel.SetActive(true);
        currentBeat = 0;
        nextBeatTime = Time.time + beatInterval;
        active = true;
        foreach (var b in beatIndicators) b.color = new Color(1, 1, 1, 0.3f);
    }

    void Update()
    {
        if (!active) return;

        if (Time.time >= nextBeatTime)
        {
            beatIndicators[currentBeat].color = Color.white;
            if (PlayerInput.InteractPressed && IsInWindow())
            {
                ahan.Calm(0.35f);
                currentBeat++;
                if (currentBeat >= beatIndicators.Length) { End(success: true); return; }
                nextBeatTime = Time.time + beatInterval;
            }
            else if (Time.time > nextBeatTime + hitWindow)
            {
                End(success: false);
            }
        }
    }

    bool IsInWindow() => Mathf.Abs(Time.time - nextBeatTime) <= hitWindow;

    void End(bool success)
    {
        active = false; panel.SetActive(false);
        if (success) { GameEvents.RaiseAhanCalmed(); GameEvents.RaiseScoreDelta(50); }
        else { ahan.cry = Mathf.Min(1f, ahan.cry + 0.2f); }
    }
}