using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameMenu : MonoBehaviour
{
    [SerializeField] TMPro.TextMeshProUGUI winnerDisplayNameText;
    [SerializeField] TMPro.TextMeshProUGUI winnerScoreDisplayText;
    [SerializeField] TopScoresManager topScoreDisplayer;


    public void DisplayEndGameMenu()
    {
        UpdateEndGameWinner();
        UpdateTopScoreDisplayer();
        this.gameObject.SetActive(true);
    }

    private void UpdateTopScoreDisplayer()
    {
        int player1Score = GameManager.GameManagerSingleton.GetPlayerScore(Players.Player1);
        int player2Score = GameManager.GameManagerSingleton.GetPlayerScore(Players.Player2);
        int bestScore = Mathf.Max(player1Score, player2Score);
        topScoreDisplayer.UpdateAndDisplayBestScore(bestScore);
    }

    private void UpdateEndGameWinner()
    {
        int player1Score = GameManager.GameManagerSingleton.GetPlayerScore(Players.Player1);
        int player2Score = GameManager.GameManagerSingleton.GetPlayerScore(Players.Player2);

        if(player1Score > player2Score)
        {
            winnerDisplayNameText.text = "Winner: Player 1";
            winnerScoreDisplayText.text = "Score: " + player1Score.ToString();
        }
        else if(player2Score > player1Score)
        {
            winnerDisplayNameText.text = "Winner: Player 2";
            winnerScoreDisplayText.text = "Score: " + player2Score.ToString();
        }
        else
        {
            winnerDisplayNameText.text = "Draw";
            winnerScoreDisplayText.text = "Both Scored: " + player1Score.ToString();
        }
    }
}
