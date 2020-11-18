using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGameUI : MonoBehaviour
{

    [SerializeField] PlayerGame playerGameRef;

    [Header("HUD Text References")]
    [SerializeField] TMPro.TextMeshProUGUI playerIndicatorText;
    [SerializeField] TMPro.TextMeshProUGUI timerText;
    [SerializeField] TMPro.TextMeshProUGUI scoreText;


    private void OnEnable()
    {
        playerIndicatorText.text = playerGameRef.Player.ToString();
        playerGameRef.OnPlayerScoreChanged.AddListener(PlayerScoreChanged);
        playerGameRef.OnPlayerTimeChanged.AddListener(PlayerTimeChanged);
        PlayerScoreChanged(playerGameRef.PlayerScore);
    }

    private void OnDisable()
    {
        playerGameRef.OnPlayerScoreChanged.RemoveListener(PlayerScoreChanged);
        playerGameRef.OnPlayerTimeChanged.RemoveListener(PlayerTimeChanged);
    }

    public void PlayerTimeChanged(int playersTime)
    {
        timerText.text = "Time: " + playersTime.ToString() + "s";
    }

    public void PlayerScoreChanged(int playersTime)
    {
        scoreText.text = "Score: " + playersTime.ToString();
    }

}
