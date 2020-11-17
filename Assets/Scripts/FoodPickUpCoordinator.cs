using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pick up coordinator for food game objects
/// </summary>
public class FoodPickUpCoordinator : HoldableCoordinator
{
    /// <summary>
    /// The food game object that can be picked up
    /// </summary>
    [Tooltip("The food game object that can be picked up")]
    [SerializeField] FoodGameObject foodGameObject;

    private void OnEnable()
    {
        // Subscribe to the food game object hold state change event
        foodGameObject.GetHoldableItemComponent().OnHoldStateChange.AddListener(OnHoldableItemHeldStateChanged);
    }

    private void OnDisable()
    {
        // Unsubscribe to the food game object hold state change event
        foodGameObject.GetHoldableItemComponent().OnHoldStateChange.RemoveListener(OnHoldableItemHeldStateChanged);
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
        return foodGameObject.GetHoldableItemComponent();
    }

    /// <summary>
    /// Returns whether or not this food game object is currently holdable
    /// </summary>
    /// <returns></returns>
    public override bool CanBeHeld()
    {
        return foodGameObject.GetHoldableItemComponent().HoldingState == HoldableItem.HeldState.Dropped;
    }
}
