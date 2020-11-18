using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pick up coordinator for holdeable game objects
/// </summary>
public class HoldeableItemPickupCoordinator : HoldableCoordinator
{
    /// <summary>
    /// The food game object that can be picked up
    /// </summary>
    [Tooltip("The holdeable game object that can be picked up")]
    [SerializeField] HoldableItem holdeableItem;

    private void OnEnable()
    {
        // Subscribe to the food game object hold state change event
        holdeableItem.OnHoldStateChange.AddListener(OnHoldableItemHeldStateChanged);
    }

    private void OnDisable()
    {
        // Unsubscribe to the food game object hold state change event
        holdeableItem.OnHoldStateChange.RemoveListener(OnHoldableItemHeldStateChanged);
    }

    /// <summary>
    /// Method called when the food game object hold state changes
    /// </summary>
    /// <param name="itemCoordinated"></param>
    private void OnHoldableItemHeldStateChanged(HoldableItem itemCoordinated)
    {
        this.onHoldableStateChange.Invoke(this as HoldableCoordinator);
    }

    /// <summary>
    /// Returns the holdable item game object for the food game object represented
    /// </summary>
    /// <returns></returns>
    public override HoldableItem GetHoldableItem()
    {
        return holdeableItem;
    }

    /// <summary>
    /// Returns whether or not this food game object is currently holdable
    /// </summary>
    /// <returns></returns>
    public override bool CanBeHeld()
    {
        return holdeableItem.HoldingState == HoldableItem.HeldState.Dropped;
    }
}
