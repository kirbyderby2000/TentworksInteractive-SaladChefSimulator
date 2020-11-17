using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for representing holdable items
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HoldableItem : MonoBehaviour
{
    /// <summary>
    /// Event invoked when the hold state of this item changes
    /// </summary>
    [Tooltip("Event invoked when the hold state of this item changes")]
    public HoldableItemChangeHandler OnHoldStateChange;

    /// <summary>
    /// The held state of a holdable item
    /// </summary>
    public enum HeldState { Held, Dropped};

    /// <summary>
    /// The held state of this holdable item (held / dropped)
    /// </summary>
    public HeldState HoldingState
    {
        private set;
        get;
    } = HeldState.Dropped;

    /// <summary>
    /// The rigidbody of this holdable game object
    /// </summary>
    private Rigidbody _holdableRB;

    private void Awake()
    {
        _holdableRB = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Method called to toggle ON / OFF IsKinematic on this food game object (Off = Non-Kinematic RB / ON = Kinematic)
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleRigidBodyKinematic(bool toggle)
    {
        // If the food rigidbody is null, then initialize it
        if (_holdableRB == null)
            _holdableRB = GetComponent<Rigidbody>();
        // Toggle the rigidbody to the toggle parameter passed
        _holdableRB.isKinematic = toggle;
    }

    /// <summary>
    /// Method that should be called when this dropped item is being held
    /// </summary>
    public void ItemHeld()
    {
        if(HoldingState == HeldState.Held)
        {
            Debug.LogError(this.gameObject.name + ".ItemHeld() CALLED BUT THIS ITEM IS ALREADY BEING HELD", this.gameObject);
        }
        HoldingState = HeldState.Held;
        OnHoldStateChange.Invoke(this);
    }

    /// <summary>
    /// Method that should be called when this held item is being dropped
    /// </summary>
    public void ItemDropped()
    {
        HoldingState = HeldState.Dropped;
        OnHoldStateChange.Invoke(this);
    }
}

/// <summary>
/// Event class used for handling holdable item state changes (Held / Dropped)
/// </summary>
[System.Serializable]
public class HoldableItemChangeHandler: UnityEngine.Events.UnityEvent<HoldableItem>
{

}
