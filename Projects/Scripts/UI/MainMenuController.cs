using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    public GameObject continueButton;

    void Start()
    {
        if (continueButton) continueButton.SetActive(SaveSystem.HasSave());
        AudioManager.Instance?.PlayMusic(AudioManager.Instance.menuMusic);
    }
    public void NewGame()
    {
        SaveSystem.Clear();
        GameManager.Instance.ResetRun();
        PlayerPrefs.DeleteKey("guard_pacified_Guard_01");  // clear per-guard state
        SceneLoader.Instance.Load("02_Intro");
    }
    public void Continue() 
    { 
        string s = SaveSystem.Load(); 
        if (!string.IsNullOrEmpty(s)) 
            SceneLoader.Instance.Load(s); 
    }
    public void Quit() 
    {
        Application.Quit();
    }
}