using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverScreen; // Assign the Game Over UI Panel in the Inspector.
    public TMPro.TextMeshProUGUI finalScoreText;

    public void GameOver(int finalScore)
    {
        gameOverScreen.SetActive(true); // Show the game-over panel.
        finalScoreText.text = "FINAL SCORE: " + finalScore; // Update the final score.
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the current scene.
    }

    public void MainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
