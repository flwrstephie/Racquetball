using UnityEngine;
using UnityEngine.UI;  
using UnityEngine.SceneManagement;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }
    public int Score { get; private set; } = 0;
    public int Lives { get; private set; } = 3; 
    public TMPro.TextMeshProUGUI scoreText;
    public Image[] heartImages;  
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddPoint()
    {
        Score++;
        UpdateUI();
    }

    public void ResetScore()
    {
        Score = 0;
        UpdateUI();
    }

    public void LoseLife()
    {
        Lives--;
        if (Lives <= 0)
        {
            SceneManager.LoadScene("GameOverScene");
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
