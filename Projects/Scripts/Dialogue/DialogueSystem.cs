using System.Collections;
using UnityEngine;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem Instance;
    public DialogueUI ui;
    public PlayerStats playerStats;

    public bool IsPlaying { get; private set; }

    void Awake() { Instance = this; }

    public void Play(DialogueNode node)
    {
        if (IsPlaying) return;
        StartCoroutine(PlayCR(node));
    }

    IEnumerator PlayCR(DialogueNode node)
    {
        IsPlaying = true;
        GameEvents.RaiseDialogueStart(node);

        foreach (var line in node.lines)
        {
            yield return ui.ShowLine(line);
        }

        if (node.choices != null && node.choices.Length > 0)
        {
            int idx = -1;
            ui.ShowChoices(node.choices, playerStats.courage, picked => idx = picked);
            while (idx < 0) yield return null;
            ApplyChoice(node.choices[idx]);
            GameEvents.RaiseChoiceMade(idx);
        }
        else if (node.nextIfNoChoice != null)
        {
            ui.Hide();
            IsPlaying = false;
            GameEvents.RaiseDialogueEnd();
            Play(node.nextIfNoChoice);
            yield break;
        }

        ui.Hide();
        IsPlaying = false;
        GameEvents.RaiseDialogueEnd();
    }

    void ApplyChoice(ChoiceOption c)
    {
        if (playerStats)
        {
            playerStats.AddCourage(c.courageDelta);
            playerStats.AddFear(c.fearDelta);
        }
        if (c.karmaDelta != 0 && GameManager.Instance != null)
            GameManager.Instance.karma = Mathf.Clamp(GameManager.Instance.karma + c.karmaDelta, -100, 100);
        if (c.scoreDelta != 0)
            GameEvents.RaiseScoreDelta(c.scoreDelta);

        // Generic dispatch by choice id prefix
        if (!string.IsNullOrEmpty(c.choiceId))
        {
            if (c.choiceId.StartsWith("guard_"))
            {
                string outcome = c.choiceId.Substring("guard_".Length); // "pass" / "fight" / "ignore"
                GameEvents.RaiseGuardOutcome(outcome);
            }
            // Future: extend with veil_, elder_, etc.
        }
    }
}