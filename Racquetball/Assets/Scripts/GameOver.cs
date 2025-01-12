using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    public TextMeshProUGUI finalScoreText;

    private void Start()
    {
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0); // Get the score, default to 0 if not found
        finalScoreText.text = "Final Score: " + finalScore;
    }

    public void RetryGame()
    {
        PlayerPrefs.DeleteKey("FinalScore"); // Optionally clear the saved score
        SceneManager.LoadScene("GameScene");
    }

    public void MainMenu()
    {
        PlayerPrefs.DeleteKey("FinalScore"); // Optionally clear the saved score
        SceneManager.LoadScene("MainMenu");
    }
}
