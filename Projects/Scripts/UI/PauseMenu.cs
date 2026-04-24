using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject panel;

    void Update()
    {
        if (PlayerInput.PausePressed) Toggle();
    }
    public void Toggle()
    {
        GameManager.Instance.TogglePause();
        panel.SetActive(GameManager.Instance.IsPaused);
    }
    public void Resume() { if (GameManager.Instance.IsPaused) Toggle(); }
    public void ToMenu() { Time.timeScale = 1f; SceneLoader.Instance.Load("01_MainMenu"); }
    public void Quit() { Application.Quit(); }
}