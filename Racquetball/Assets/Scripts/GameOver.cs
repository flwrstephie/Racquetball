using UnityEngine;
using TMPro;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    private void Start()
    {
        if (ScoreManager.Instance != null)
        {
            finalScoreText.text = "Final Score: " + ScoreManager.Instance.Score;
        }
    }

    public void RetryGame()
    {
        ScoreManager.Instance.ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        ScoreManager.Instance.ResetScore();
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
