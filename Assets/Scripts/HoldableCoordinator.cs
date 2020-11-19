using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract class used for coordinating hodable items that can be picked up
/// </summary>
public abstract class HoldableCoordinator : MonoBehaviour
{
    /// <summary>
    /// Event invoked when this coordinator's holdable state changes
    /// </summary>
    [Tooltip("Event invoked when this coordinator's holdable state changes")]
    public HoldableCoordinatorStateChange onHoldableStateChange;

    /// <summary>
    /// Returns whether or not this item can be picked up
    /// </summary>
    /// <returns></returns>
    public virtual bool CanBeHeld()
    {
        return this.gameObject.activeInHierarchy;
    }

    /// <summary>
    /// Returns the holdable item of this Holdable Item Proc signal
    /// </summary>
    /// <returns></returns>
    public abstract HoldableItem GetHoldableItem();
}

/// <summary>
/// Event handler used when the holdable state of a coordinator changes 
/// </summary>
[System.Serializable]
public class HoldableCoordinatorStateChange : UnityEngine.Events.UnityEvent<HoldableCoordinator>
{

}
