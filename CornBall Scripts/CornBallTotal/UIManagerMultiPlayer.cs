using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManagerMultiPlayer : MonoBehaviour
{
    public static UIManagerMultiPlayer Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }
   [Header("In-Game UI")]
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI turnText;
    [SerializeField] private TextMeshProUGUI bagsRemainingText;

    [Header("Panels")]
    [SerializeField] private GameObject turnPanel;
    [SerializeField] private TextMeshProUGUI turnPanelText;
    [SerializeField] private GameObject endOfRoundPanel;
    [SerializeField] private TextMeshProUGUI endOfRoundSummaryText;

    void Start()
    {
        turnPanel.SetActive(false);
        endOfRoundPanel.SetActive(false);
    }

    public void UpdatePlayerScores(int p1Score, int p2Score)
    {
        player1ScoreText.text = $"Player 1: {p1Score}";
        player2ScoreText.text = $"Player 2: {p2Score}";
    }

    public void SetTurnText(string text)
    {
        turnText.text = text;
    }

    public void UpdateBagsRemaining(int count)
    {
        bagsRemainingText.text = $"Bags Left: {count}";
    }

    /// <summary>
    /// Shows the panel with a message (e.g., "Player 2's Turn").
    /// </summary>
    /// 
    public void ShowTurnPanel(string message)
    {
        turnPanelText.text = message;
        turnPanel.SetActive(true);
    }

    /// <summary>
    /// Hides the turn panel. Called by the GameManager.
    /// </summary>
    public void HideTurnPanel()
    {
        turnPanel.SetActive(false);
    }

    public void ShowEndOfRoundPanel(int p1Score, int p2Score)
    {
        endOfRoundSummaryText.text = $"End of Round!\n\nPlayer 1: {p1Score}\nPlayer 2: {p2Score}";
        endOfRoundPanel.SetActive(true);
    }


    public void OnStartNextRoundButtonPressed()
    {
        endOfRoundPanel.SetActive(false);
        GameManagerMultiplayer.Instance.StartNewRound();
    }
}
