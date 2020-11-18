using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Interactable
{
    [Min(1)]
    [SerializeField] int scoreToDeductFromPlayer = 1;

    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {
        FoodGameObject droppedFood = droppedItem.GetComponent<FoodGameObject>();

        if(droppedFood != null)
        {
            Destroy(droppedItem.gameObject);
            GameManager.GameManagerSingleton.ModifyPlayerScore(playerThatDroppedTheItem.Player, -scoreToDeductFromPlayer);
        }
    }
}
