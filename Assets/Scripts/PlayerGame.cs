using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class used for managing a player's game (player's score and timer)
/// </summary>
public class PlayerGame : MonoBehaviour
{
    /// <summary>
    /// The player that this PlayerGame instance represents
    /// </summary>
    [SerializeField] Players player;

    [Tooltip("Event raised when the player's time changes")]
    public NumberChangedEvent OnPlayerTimeChanged;

    [Tooltip("Event raised when the player runs out of time")]
    public UnityEngine.Events.UnityEvent OnPlayerRanOutOfTime;

    [Tooltip("Event raised when the player's score changes")]
    public NumberChangedEvent OnPlayerScoreChanged;

    /// <summary>
    /// The player's time
    /// </summary>
    public int PlayerTime
    {
        private set;
        get;
    } = 0;

    /// <summary>
    /// The player's score
    /// </summary>
    public int PlayerScore
    {
        private set;
        get;
    } = 0;
    

    /// <summary>
    /// Method called to start the player's timer
    /// </summary>
    /// <param name="timeAllotted">The time allotted for this player</param>
    public void StartPlayerTimer(int timeAllotted)
    {
        if(playerTimerCoroutine != null)
        {
            Debug.LogWarning("Player timer coroutine already started!", this.gameObject);
            return;
        }
        // Assign the player timer value to the time allotted
        PlayerTime = timeAllotted;
        // Raise the player time changed event
        OnPlayerTimeChanged.Invoke(PlayerTime);
        // Assign the cached player timer coroutine to the player timer
        playerTimerCoroutine = StartCoroutine(PlayerTimer());
    }

    /// <summary>
    /// The cached player timer coroutine
    /// </summary>
    Coroutine playerTimerCoroutine = null;

    /// <summary>
    /// Player timer IEnumerator coroutine method
    /// </summary>
    /// <returns></returns>
    IEnumerator PlayerTimer()
    {
        // While the player time is more than 0, 
        // Wait 1 second and then decrement the timer by 1 
        while(PlayerTime > 0)
        {
            // Wait for 1 second
            yield return new WaitForSeconds(1.0f);
            // Decrement the player timer by -1
            ModifyPlayerTime(-1);
        }
        // Raise the OnPlayerRanOutOfTime event
        OnPlayerRanOutOfTime.Invoke();
    }

    /// <summary>
    /// Method called to modify the player's time
    /// </summary>
    /// <param name="timeToAdd"></param>
    public void ModifyPlayerTime(int timeToAdd)
    {
        // If the player's time is less than or equal to 0, then do nothing
        if (PlayerTime <= 0)
            return;
        // Modify the player's time by the parameter passed
        PlayerTime += timeToAdd;

        // If the player's time is less than or equal to 0, then set the player time to 0
        if (PlayerTime < 0)
            PlayerTime = 0;
        // Raise the player time changed event
        OnPlayerTimeChanged.Invoke(PlayerTime);
    }
    
    /// <summary>
    /// Method called to modify the player's score
    /// </summary>
    /// <param name="scoreToAdd"></param>
    public void ModifyPlayerScore(int scoreToAdd)
    {
        // Add the score passed to the player's score
        PlayerScore += scoreToAdd;
        // Raise the player score changed event
        OnPlayerScoreChanged.Invoke(PlayerScore);
    }
}

/// <summary>
/// Enum used to differentiate players
/// </summary>
public enum Players { Player1, Player2 };

/// <summary>
/// Event used when a number has changed
/// </summary>
[System.Serializable]
public class NumberChangedEvent: UnityEngine.Events.UnityEvent<int>
{

}
