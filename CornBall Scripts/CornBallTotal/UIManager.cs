using UnityEngine;
using TMPro;

/// <summary>
/// Handles all UI updates. It gets data from other scripts but doesn't manage game state itself.
/// </summary>
public class UIManager : MonoBehaviour
{
    // --- UI Element References ---
    // Assign these in the Unity Inspector
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField]private TextMeshProUGUI coinsText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private GameObject gameOverPanel;

    void Start()
    {
        // Ensure the game over panel is hidden at the start
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the score display on the screen.
    /// </summary>
    /// <param name="score">The new score to display.</param>
    public void UpdateScoreText(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }

    /// <summary>
    /// Updates the coins display on the screen.
    /// </summary>
    /// <param name="coins">The new coin count to display.</param>
    public void UpdateCoinsText(int coins)
    {
        if (coinsText != null)
        {
            coinsText.text = "Coins: " + coins;
        }
    }

    /// <summary>
    /// Updates the timer display on the screen.
    /// </summary>
    /// <param name="time">The time remaining.</param>
    public void UpdateTimerText(float time)
    {
        if (timerText != null)
        {
            // Ensure time doesn't display as negative
            if (time < 0) time = 0;
            // Format to a whole number
            timerText.text = "Time: " + time.ToString("0");
        }
    }

    /// <summary>
    /// Makes the Game Over panel visible.
    /// </summary>
    public void ShowGameOverPanel()
    {
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }
}
