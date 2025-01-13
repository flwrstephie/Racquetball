using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverHandler : MonoBehaviour
{
    public GameObject gameOverScreen; 
    public TMPro.TextMeshProUGUI finalScoreText;

    public void GameOver(int finalScore)
    {
        gameOverScreen.SetActive(true); 
        finalScoreText.text = "FINAL SCORE: " + finalScore; 
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); 
    }

    public void MainMenuScreen()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
