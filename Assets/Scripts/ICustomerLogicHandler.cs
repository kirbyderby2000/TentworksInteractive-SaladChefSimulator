using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Interface used for handling customer logic
/// </summary>
public interface ICustomerLogicHandler
{
    /// <summary>
    /// Method called to reward a given player with a customer
    /// </summary>
    /// <param name="player">The player to reward</param>
    /// <param name="customer">The customer rewarding the player</param>
    void RewardPlayer(Players player, Customer customer);
    /// <summary>
    /// Method called to punish players with a given customer
    /// </summary>
    /// <param name="customer"></param>
    void PunishPlayers(Customer customer);
}

/// <summary>
/// Test class for the customer logic
/// </summary>
public class TestCustomerLogicHandler : ICustomerLogicHandler
{
    public void PunishPlayers(Customer customer)
    {
        List<Players> playersToPunish = customer.AngeredByPlayers;

        if(playersToPunish.Count == 0)
        {
            playersToPunish.Add(Players.Player1);
            playersToPunish.Add(Players.Player2);
        }
        else
        {
            playersToPunish = playersToPunish.Distinct().ToList();
        }

        string logMessage = "Punishing players ";

        foreach (Players player in playersToPunish)
        {
            logMessage += player.ToString() + ", ";
        }

        logMessage += "; Ingredient order count penalty: " + customer.CustomerOrder.Count() + "; Ingredient Order: ";

        foreach (Ingredient item in customer.CustomerOrder)
        {
            logMessage += item.IngredientName + ", ";
        }

        logMessage += "; Customer State: " + customer.CurrentState.ToString();

        Debug.Log(logMessage);
    }

    public void RewardPlayer(Players player, Customer customer)
    {
        string logMessage = "Rewarding players " + player.ToString();
        logMessage += "; Ingredient order count reward: " + customer.CustomerOrder.Count() + "; Ingredient Order: ";
        foreach (Ingredient item in customer.CustomerOrder)
        {
            logMessage += item.IngredientName + ", ";
        }

        logMessage += "; Time Value: " + customer.CustomerTimeLeft.ToString();
        

        Debug.Log(logMessage);
    }
}