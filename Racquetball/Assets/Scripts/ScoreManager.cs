using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3;
    public TMPro.TextMeshProUGUI scoreText;
    public Image[] heartImages;
    public GameOverHandler gameOverHandler;  // Reference to the game-over handler.

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        ResetScoreAndLives();
    }

    public void AddPoint()
    {
        Score++;
        UpdateUI();
    }

    public void ResetScoreAndLives()
    {
        Score = 0;
        Lives = 3;
        UpdateUI();
        UpdateLivesUI();
    }

    public void LoseLife()
    {
        Lives--;
        if (Lives <= 0)
        {
            gameOverHandler.GameOver(Score);
        }
        UpdateLivesUI();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (scoreText != null) scoreText.text = "Score: " + Score;
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < Lives;
        }
    }
}
