using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Overall base class used for managing the game logic
/// </summary>
public class GameManager : MonoBehaviour
{
    [Tooltip("How much time each player will be initially allotted")]
    [SerializeField] int playerTimeAllotted = 180;

    [Header("Player Game Script References")]
    [Tooltip("The player 1 game reference")]
    [SerializeField] PlayerGame player1Game;
    [Tooltip("The player 2 game reference")]
    [SerializeField] PlayerGame player2Game;

    [Header("Test Settings")]
    [SerializeField] bool testGameManager = false;

    [Tooltip("Event raised when a game starts")]
    /// <summary>
    /// Event raised when a game starts
    /// </summary>
    [SerializeField] UnityEngine.Events.UnityEvent OnGameStarted;

    [Tooltip("Event raised when a game ends")]
    /// <summary>
    /// Event raised when a game ends
    /// </summary>
    [SerializeField] UnityEngine.Events.UnityEvent OnGameEnded;

    /// <summary>
    /// Control variable to cache whether or not a game has started on this game manager
    /// </summary>
    private bool _gameStarted = false;

    /// <summary>
    /// The active game manager singleton in the scene
    /// </summary>
    public static GameManager GameManagerSingleton
    {
        private set;
        get;
    }

    private void Awake()
    {
        // If the game manager is not null, then destroy this game manager instance
        if(GameManagerSingleton != null)
        {
            Destroy(this.gameObject);
        }
        // Otherwise, assign the singleton property to this instance
        else
        {
            GameManagerSingleton = this;
#if UNITY_EDITOR
            if (testGameManager)
            {
                StartGame();
            }
#endif


        }
    }

    private void OnEnable()
    {
        // When the game manager is enabled, add a listener to the player 1 and player 2 
        // time-ran-out events
        player1Game.OnPlayerRanOutOfTime.AddListener(OnGamePlayerTimerRanOut);
        player2Game.OnPlayerRanOutOfTime.AddListener(OnGamePlayerTimerRanOut);
    }

    private void OnDisable()
    {
        // When the game manager is disabled, remove the listener from the player 1 and player 2
        // time-ran-out events
        player1Game.OnPlayerRanOutOfTime.RemoveListener(OnGamePlayerTimerRanOut);
        player2Game.OnPlayerRanOutOfTime.RemoveListener(OnGamePlayerTimerRanOut);
    }


    /// <summary>
    /// Method called to start the game
    /// </summary>
    public void StartGame()
    {
        if (_gameStarted)
        {
            Debug.LogWarning("StartGame was called but a game was already started", this.gameObject);
            return;
        }
        float gameStartTime = Time.time;
        player1Game.StartPlayerTimer(gameStartTime, playerTimeAllotted);
        player2Game.StartPlayerTimer(gameStartTime, playerTimeAllotted);
        Debug.Log("Game Started");
        // Event raised when a game starts
        OnGameStarted.Invoke();
        _gameStarted = true;
    }

    /// <summary>
    /// Method called when a player runs out of time (Called when a player game "TimeRanOut" event is raised)
    /// </summary>
    private void OnGamePlayerTimerRanOut()
    {
        // If both the timers for both players is less than or equal to 0,
        // Then call the game ended event
        if(player1Game.PlayerTime <= 0 && player2Game.PlayerTime <= 0)
        {
            Debug.Log("Game Ended");
            OnGameEnded.Invoke();
        }
    }

    /// <summary>
    /// Method called to modify a player's score
    /// </summary>
    /// <param name="player"></param>
    /// <param name="scoreToAdd"></param>
    public void ModifyPlayerScore(Players player, int scoreToAdd)
    {
        GetPlayerGame(player).ModifyPlayerScore(scoreToAdd);
    }

    /// <summary>
    /// Method called to modify a player's timer
    /// </summary>
    /// <param name="player"></param>
    /// <param name="timeToAdd"></param>
    public void ModifyPlayerTimer(Players player, int timeToAdd)
    {
        GetPlayerGame(player).ModifyPlayerTime(timeToAdd);
    }

    /// <summary>
    /// Returns the proper player game
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    private PlayerGame GetPlayerGame(Players player)
    {
        return player == Players.Player1 ? player1Game : player2Game;
    }

    /// <summary>
    /// Returns the proper player's score
    /// </summary>
    /// <param name="player"></param>
    /// <returns></returns>
    public int GetPlayerScore(Players player)
    {
        return GetPlayerGame(player).PlayerScore;
    }

}

