using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisposableSalad : Disposable
{
    [SerializeField] SaladBowl saladBow;



    public override void Dispose()
    {
        saladBow.ConsumeBowl();
    }

    public override int PointsDeductedForDisposing()
    {
        return saladBow.SaladIngredients.Count;
    }
}
