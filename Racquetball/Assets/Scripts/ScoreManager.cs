using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3; // Start with 3 lives

    // Reference to the Text components
    public TMPro.TextMeshProUGUI scoreText;
    public TMPro.TextMeshProUGUI livesText;
    
    // Reference to the Game Over UI elements
    public GameObject gameOverUI; // The Game Over Canvas
    public TMPro.TextMeshProUGUI gameOverText; // Game Over Text

    private bool isGameOver = false;

    private void Awake()
    {
        // Ensure only one instance exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Persist between scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoint()
    {
        if (!isGameOver)
        {
            Score++;
            UpdateUI();
        }
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    public void LoseLife()
    {
        if (!isGameOver)
        {
            Lives--;
            if (Lives <= 0)
            {
                // Game Over Logic
                TriggerGameOver();
            }
            UpdateUI();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;

        // Show the Game Over UI
        gameOverUI.SetActive(true);
        gameOverText.text = "Game Over! Final Score: " + Score;

        // Optionally, you can freeze the background or stop all actions here
        // For example, stopping player movement
        Time.timeScale = 0; // Freezes everything in the game
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + Score;
        if (livesText != null) livesText.text = "Lives: " + Lives;
    }
}
