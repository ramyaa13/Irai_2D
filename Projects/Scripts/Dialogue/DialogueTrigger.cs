using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class DialogueTrigger : MonoBehaviour
{
    public DialogueNode node;
    public bool oncePerScene = true;
    bool fired;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (fired && oncePerScene) return;
        if (!other.CompareTag("Player")) return;
        if (!DialogueSystem.Instance) return;
        DialogueSystem.Instance.Play(node);
        fired = true;

        Debug.Log($"Triggered dialogue: player entered");
    }
}