using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Disposable : MonoBehaviour
{
    public abstract void Dispose(PlayerController playerDisposingItem);

    public virtual int PointsDeductedForDisposing()
    {
        return 1;
    }
}
