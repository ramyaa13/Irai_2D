using UnityEngine;

public static class SaveSystem
{
    const string K_LAST_SCENE = "irai_last_scene";
    const string K_KARMA = "irai_karma";
    const string K_SCORE = "irai_score";
    const string K_SHARDS = "irai_shards";
    const string K_HAS_SAVE = "irai_has_save";

    public static bool HasSave() => PlayerPrefs.GetInt(K_HAS_SAVE, 0) == 1;

    public static void Save(string scene)
    {
        PlayerPrefs.SetString(K_LAST_SCENE, scene);
        PlayerPrefs.SetInt(K_KARMA, GameManager.Instance.karma);
        PlayerPrefs.SetInt(K_SCORE, GameManager.Instance.totalScore);
        PlayerPrefs.SetInt(K_SHARDS, GameManager.Instance.memoryShards);
        PlayerPrefs.SetInt(K_HAS_SAVE, 1);
        PlayerPrefs.Save();
    }

    public static string Load()
    {
        if (!HasSave()) return null;
        GameManager.Instance.karma = PlayerPrefs.GetInt(K_KARMA);
        GameManager.Instance.totalScore = PlayerPrefs.GetInt(K_SCORE);
        GameManager.Instance.memoryShards = PlayerPrefs.GetInt(K_SHARDS);
        return PlayerPrefs.GetString(K_LAST_SCENE);
    }

    public static void Clear() { PlayerPrefs.DeleteAll(); }
}