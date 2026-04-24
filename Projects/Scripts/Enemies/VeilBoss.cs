using System.Collections;
using UnityEngine;

public class VeilBoss : MonoBehaviour
{
    public DialogueNode openingNode;
    public DialogueNode denyEndingNode;
    public DialogueNode submitEndingNode;
    public DialogueNode acceptEndingNode;

    public string endingSceneName = "07_Ending";

    void Start()
    {
        StartCoroutine(Run());
    }

    IEnumerator Run()
    {
        yield return new WaitForSeconds(1f);
        DialogueSystem.Instance.Play(openingNode);

        // Opening node should have 3 choices: 0 = Deny, 1 = Submit, 2 = Accept
        int pick = -1;
        System.Action<int> handler = i => pick = i;
        GameEvents.OnChoiceMade += handler;

        while (pick < 0) yield return null;
        GameEvents.OnChoiceMade -= handler;

        yield return new WaitForSeconds(0.5f);

        switch (pick)
        {
            case 0:
                GameManager.Instance.ending = GameManager.EndingType.Deny;
                DialogueSystem.Instance.Play(denyEndingNode); break;
            case 1:
                GameManager.Instance.ending = GameManager.EndingType.Submit;
                DialogueSystem.Instance.Play(submitEndingNode); break;
            default:
                GameManager.Instance.ending = GameManager.EndingType.Accept;
                DialogueSystem.Instance.Play(acceptEndingNode); break;
        }

        while (DialogueSystem.Instance.IsPlaying) yield return null;

        yield return new WaitForSeconds(1.5f);
        SceneLoader.Instance.Load(endingSceneName);
    }
}