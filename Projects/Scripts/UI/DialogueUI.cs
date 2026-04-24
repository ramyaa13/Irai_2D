using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueUI : MonoBehaviour
{
    public CanvasGroup panel;
    public TMP_Text speakerText;
    public TMP_Text bodyText;
    public float charsPerSec = 40f;

    [Header("Choices")]
    public GameObject choicePanel;
    public Button[] choiceButtons;       // prefabs with TMP child

    Action<int> onPicked;

    void Awake() { Hide(); }

    public IEnumerator ShowLine(DialogueLine line)
    {
        panel.alpha = 1; panel.blocksRaycasts = true;
        speakerText.text = line.speaker;
        bodyText.text = "";

        if (line.voice) AudioManager.Instance?.sfxSource.PlayOneShot(line.voice);

        float t = 0f;
        while (t < line.text.Length / charsPerSec)
        {
            t += Time.deltaTime;
            int n = Mathf.Min(line.text.Length, Mathf.FloorToInt(t * charsPerSec));
            bodyText.text = line.text.Substring(0, n);
            if (PlayerInput.InteractPressed) { bodyText.text = line.text; break; }
            yield return null;
        }
        bodyText.text = line.text;

        // wait for confirm
        while (!PlayerInput.InteractPressed) yield return null;
        yield return new WaitForSeconds(line.extraDelay);
    }

    public void ShowChoices(ChoiceOption[] options, float currentCourage, Action<int> picked)
    {
        onPicked = picked;
        choicePanel.SetActive(true);

        for (int i = 0; i < choiceButtons.Length; i++)
        {
            if (i < options.Length)
            {
                int idx = i;
                var btn = choiceButtons[i];
                btn.gameObject.SetActive(true);
                var label = btn.GetComponentInChildren<TMP_Text>();
                label.text = options[i].label;
                bool locked = options[i].minCourageRequired > currentCourage;
                btn.interactable = !locked;
                if (locked) label.text += "  (need " + options[i].minCourageRequired + " courage)";
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => OnPick(idx));
            }
            else choiceButtons[i].gameObject.SetActive(false);
        }
    }

    void OnPick(int i) { choicePanel.SetActive(false); onPicked?.Invoke(i); }

    public void Hide()
    {
        if (!panel) return;
        panel.alpha = 0; panel.blocksRaycasts = false;
        if (choicePanel) choicePanel.SetActive(false);
    }
}