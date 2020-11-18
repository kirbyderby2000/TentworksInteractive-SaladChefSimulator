using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispoableFood : Disposable
{
    public override void Dispose()
    {
        Destroy(this.gameObject);
    }
}
