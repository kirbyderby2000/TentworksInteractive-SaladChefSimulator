using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trashcan : Interactable
{

    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {
        Disposable disposableItem = droppedItem.GetComponent<Disposable>();

        if(disposableItem != null)
        {
            GameManager.GameManagerSingleton.ModifyPlayerScore(playerThatDroppedTheItem.Player, - disposableItem.PointsDeductedForDisposing());
            disposableItem.Dispose(playerThatDroppedTheItem);
        }
    }
}
