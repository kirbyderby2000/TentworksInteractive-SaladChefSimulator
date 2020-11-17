using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Interactable
{
    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {
        FoodGameObject droppedFood = droppedItem.GetComponent<FoodGameObject>();

        if(droppedFood != null)
        {
            Destroy(droppedItem.gameObject);
        }
    }
}
