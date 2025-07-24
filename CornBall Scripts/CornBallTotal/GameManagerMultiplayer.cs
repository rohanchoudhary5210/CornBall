using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Updated to correctly destroy old bags when a new round starts.
/// </summary>
public class GameManagerMultiplayer : MonoBehaviour
{
    public static GameManagerMultiplayer Instance { get; private set; }

    // --- Dependencies (Assign in Inspector) ---
    [SerializeField] private UIManagerMultiPlayer uiManager;
    [SerializeField] private SpawnMangerMultiPlayer spawnManager;

    // --- Game State ---
    private int _player1Score = 0;
    private int _player2Score = 0;
    private int _currentPlayer = 1;
    private int _bagsThrownThisTurn = 0;
    private const int BAGS_PER_ROUND = 4;
    private List<GameObject> _bagsInPlay = new List<GameObject>();

    void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
        } else {
            Instance = this;
        }
    }

    void Start()
    {
        StartNewRound();
    }

    public void RecordThrow(int points, GameObject bag)
    {
        _bagsInPlay.Add(bag);

        if (_currentPlayer == 1) {
            _player1Score += points;
        } else {
            _player2Score += points;
        }

        _bagsThrownThisTurn++;
        UpdateUI();
        
        StartCoroutine(HandleNextAction());
    }

    private IEnumerator HandleNextAction()
    {
        yield return new WaitForSeconds(1.5f);

        if (_bagsThrownThisTurn < BAGS_PER_ROUND)
        {
            spawnManager.SpawnSandbag(_currentPlayer);
        }
        else
        {
            if (_currentPlayer == 1)
            {
                StartCoroutine(SwitchPlayerSequence());
            }
            else
            {
                EndRound();
            }
        }
    }
    
    private IEnumerator SwitchPlayerSequence()
    {
        _currentPlayer = 2;
        _bagsThrownThisTurn = 0;
        
        uiManager.ShowTurnPanel("Player 2's Turn");
        yield return new WaitForSeconds(2.5f);
        
        uiManager.HideTurnPanel();
        UpdateUI();
        
        spawnManager.SpawnSandbag(_currentPlayer);
    }

    private void EndRound()
    {
        uiManager.ShowEndOfRoundPanel(_player1Score, _player2Score);
    }

    /// <summary>
    /// *** BUG FIX ***
    /// The code to destroy the bags from the previous round has been added back.
    /// This will clear the board before a new round begins.
    /// </summary>
    public void StartNewRound()
    {
        // Loop through all the bags we tracked from the last round and destroy them.
        foreach (GameObject bag in _bagsInPlay)
        {
            // Check if the bag hasn't been destroyed already for safety.
            if (bag != null) {
                Destroy(bag);
            }
        }
        // Clear the list to prepare for tracking the new bags.
        _bagsInPlay.Clear();

        _currentPlayer = 1;
        _bagsThrownThisTurn = 0;

        UpdateUI();
        spawnManager.SpawnSandbag(_currentPlayer);
    }

    private void UpdateUI()
    {
        uiManager.UpdatePlayerScores(_player1Score, _player2Score);
        uiManager.SetTurnText($"Player {_currentPlayer}'s Turn");
        uiManager.UpdateBagsRemaining(BAGS_PER_ROUND - _bagsThrownThisTurn);
    }
}
