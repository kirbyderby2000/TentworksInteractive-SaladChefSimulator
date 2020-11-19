using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableSalad : Disposable
{
    [SerializeField] SaladBowl saladBow;



    public override void Dispose(PlayerController playerThatDroppedTheItem)
    {
        saladBow.ConsumeBowl();
        playerThatDroppedTheItem.PlayerHand.HoldItem(GetComponent<HoldableItem>());
    }

    public override int PointsDeductedForDisposing()
    {
        return saladBow.SaladIngredients.Count;
    }
}
