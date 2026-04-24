using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class SceneExitTrigger : MonoBehaviour
{
    public string nextSceneName;
    public bool requireAhanCalm = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;
        var ahan = other.GetComponentInChildren<AhanSystem>();
        if (requireAhanCalm && ahan && ahan.cry > 0.4f)
        {
            UIManager.Instance?.Toast("Ahan is not calm enough to travel.");
            return;
        }
        GameEvents.RaiseSceneCompleted(nextSceneName);
        SceneLoader.Instance.Load(nextSceneName);
    }
}