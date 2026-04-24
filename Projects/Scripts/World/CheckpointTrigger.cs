using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CheckpointTrigger : MonoBehaviour
{
    public string checkpointId;
    public string sceneNameForSave;
    bool used;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (used || !other.CompareTag("Player")) return;
        used = true;
        GameEvents.RaiseCheckpoint(checkpointId);
        SaveSystem.Save(sceneNameForSave);
        UIManager.Instance?.Toast("Checkpoint");
    }
}