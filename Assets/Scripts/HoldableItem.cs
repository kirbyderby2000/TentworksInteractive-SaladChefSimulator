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
}
