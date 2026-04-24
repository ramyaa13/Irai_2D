using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResultsScreen : MonoBehaviour
{
    public TMP_Text titleText;
    public TMP_Text bodyText;
    public Image artImage;
    public Sprite artDeny, artSubmit, artAccept;

    void Start()
    {
        var gm = GameManager.Instance;
        string title = "";
        string body = "";
        Sprite art = artAccept;

        switch (gm.ending)
        {
            case GameManager.EndingType.Deny:
                title = "She refused her shadow.";
                body = "Irai reached the mountain. The Veil still walks behind her.\nYou chose denial.";
                art = artDeny;
                break;
            case GameManager.EndingType.Submit:
                title = "She lay down in the dark.";
                body = "The mountain was always too far.\nYou chose surrender.";
                art = artSubmit;
                break;
            default: // Accept
                title = "She walked with her shadow.";
                body = "Ahan opened his eyes to sunrise.\nYou chose to carry fear, not fight it.";
                art = artAccept;
                break;
        }

        titleText.text = title;
        bodyText.text =
            body +
            $"\n\nScore: {gm.totalScore}" +
            $"\nKarma: {gm.karma}" +
            $"\nMemory Shards: {gm.memoryShards}/5" +
            $"\nCollectibles: {gm.collectiblesFound}" +
            $"\nDeaths: {gm.deaths}";
        if (artImage) artImage.sprite = art;
    }

    public void PlayAgain() { GameManager.Instance.ResetRun(); SceneLoader.Instance.Load("02_Intro"); }
    public void ToMenu() { GameManager.Instance.ResetRun(); SceneLoader.Instance.Load("01_MainMenu"); }
}