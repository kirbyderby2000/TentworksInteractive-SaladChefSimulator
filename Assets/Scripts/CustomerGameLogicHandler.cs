using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CustomerGameLogicHandler : MonoBehaviour, ICustomerLogicHandler
{
    [Min(1)]
    [SerializeField] int scorePerIngredientOnOrder = 1;

    public void PunishPlayers(Customer customer)
    {
        List<Players> playersToPunish = customer.AngeredByPlayers.Distinct().ToList();

        if (playersToPunish.Count == 0)
        {
            playersToPunish.Add(Players.Player1);
            playersToPunish.Add(Players.Player2);
        }

        foreach (Players player in playersToPunish)
        {
            GameManager.GameManagerSingleton.ModifyPlayerScore(player, GetScoreToPunishPlayerBy(customer));
        }
    }

    private int GetScoreToPunishPlayerBy(Customer customer)
    {
        int punishScore = GetScoreToReward(customer) * -1;

        if (customer.CurrentState == Customer.CustomerState.Angry)
            punishScore *= 2;
        return punishScore;
    }

    private int GetScoreToReward(Customer customer)
    {
        int scoreToReward = customer.CustomerOrder.Count * scorePerIngredientOnOrder;
        return scoreToReward;
    }

    public void RewardPlayer(Players player, Customer customer)
    {
        GameManager.GameManagerSingleton.ModifyPlayerScore(player, GetScoreToReward(customer));

        if(customer.CustomerTimeLeft >= 0.7f)
        {
            OnPlayerShouldBeRewardedPickUp.Invoke(player);
        }
    }

    [Tooltip("Event invoked when a player should be rewarded a pick-up")]
    [SerializeField] PlayerEventHandler OnPlayerShouldBeRewardedPickUp;
}

[System.Serializable]
public class PlayerEventHandler: UnityEngine.Events.UnityEvent<Players>
{

}