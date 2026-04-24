using System.Collections;
using UnityEngine;

public class BootLoader : MonoBehaviour
{
    public string firstSceneName = "01_MainMenu";
    public float holdBlackSeconds = 0.3f;

    IEnumerator Start()
    {
        // Wait one frame so all persistent managers' Awake/OnEnable run first
        yield return null;
        yield return new WaitForSeconds(holdBlackSeconds);

        if (SceneLoader.Instance == null)
        {
            Debug.LogError("[BootLoader] SceneLoader.Instance is null. " +
                           "Check that the SceneLoader GameObject exists in Boot scene.");
            yield break;
        }
        SceneLoader.Instance.Load(firstSceneName);
    }
}