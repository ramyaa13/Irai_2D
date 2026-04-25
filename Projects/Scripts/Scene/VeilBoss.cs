using System.Collections;
using UnityEngine;

public class VeilBoss : MonoBehaviour
{
    [Header("Dialogue Tree")]
    public DialogueNode reveal;             // Stage 1
    public DialogueNode accusation;         // Stage 2 (branches from here)
    public DialogueNode memoryDefy;
    public DialogueNode memoryListen;
    public DialogueNode memoryBow;
    public DialogueNode finalChoice;        // Stage 4
    public DialogueNode endingDeny;
    public DialogueNode endingSubmit;
    public DialogueNode endingAccept;

    [Header("Flow")]
    public string endingSceneName = "07_Ending";
    public float gapBetweenStages = 1.0f;
    public float endingFadeDelay = 2.5f;

    [Header("Scene Refs")]
    public Transform veilVisual;            // Veil_Visual transform
    public CanvasGroup endingCanvasGroup;   // Canvas_FinalEnding (alpha 0)

    int firstChoice = -1;       // 0=defy, 1=listen, 2=bow
    int finalChoiceIdx = -1;    // 0=deny, 1=submit, 2=accept
    int currentStage = 0;       // tracks which choice we're listening for

    public void BeginEncounter()
    {
        StartCoroutine(RunEncounter());
    }

    IEnumerator RunEncounter()
    {
        // Subscribe to choice events
        GameEvents.OnChoiceMade += HandleChoice;

        // Stage 1: Reveal (no choice)
        currentStage = 0;
        yield return PlayAndWait(reveal);
        yield return new WaitForSeconds(gapBetweenStages);

        // Stage 2: Accusation (first choice)
        currentStage = 1;
        firstChoice = -1;
        yield return PlayAndWait(accusation);
        while (firstChoice < 0) yield return null;   // wait for choice
        yield return new WaitForSeconds(gapBetweenStages);

        // Stage 3: Memory branch
        currentStage = 2;
        DialogueNode memNode = firstChoice switch
        {
            0 => memoryDefy,
            1 => memoryListen,
            2 => memoryBow,
            _ => memoryListen
        };
        yield return PlayAndWait(memNode);
        yield return new WaitForSeconds(gapBetweenStages);

        // Stage 4: Final Choice
        currentStage = 3;
        finalChoiceIdx = -1;
        yield return PlayAndWait(finalChoice);
        while (finalChoiceIdx < 0) yield return null;
        yield return new WaitForSeconds(gapBetweenStages);

        // Stage 5: Ending dialogue
        currentStage = 4;
        DialogueNode endingNode;
        switch (finalChoiceIdx)
        {
            case 0:
                GameManager.Instance.ending = GameManager.EndingType.Deny;
                endingNode = endingDeny;
                break;
            case 1:
                GameManager.Instance.ending = GameManager.EndingType.Submit;
                endingNode = endingSubmit;
                break;
            default:
                GameManager.Instance.ending = GameManager.EndingType.Accept;
                endingNode = endingAccept;
                break;
        }
        yield return PlayAndWait(endingNode);

        GameEvents.OnChoiceMade -= HandleChoice;

        // Hand off to Uyir Malai cinematic
        yield return new WaitForSeconds(endingFadeDelay);
        yield return PlayUyirMalaiSequence();
    }

    void HandleChoice(int idx)
    {
        if (currentStage == 1) firstChoice = idx;
        else if (currentStage == 3) finalChoiceIdx = idx;
    }

    IEnumerator PlayAndWait(DialogueNode node)
    {
        if (!node || DialogueSystem.Instance == null) yield break;
        DialogueSystem.Instance.Play(node);
        yield return new WaitForSeconds(0.1f);
        while (DialogueSystem.Instance.IsPlaying) yield return null;
    }

    IEnumerator PlayUyirMalaiSequence()
    {
        // Show ending cinematic canvas
        if (endingCanvasGroup)
        {
            float t = 0f;
            while (t < 1.5f)
            {
                t += Time.deltaTime;
                endingCanvasGroup.alpha = Mathf.Clamp01(t / 1.5f);
                yield return null;
            }
            endingCanvasGroup.alpha = 1f;
        }

        // Hand off to actual ending scene after cinematic plays
        yield return new WaitForSeconds(6f);
        if (SceneLoader.Instance != null)
            SceneLoader.Instance.Load(endingSceneName);
    }
}