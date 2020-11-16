using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for food game objects
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class FoodGameObject : MonoBehaviour
{
    [Tooltip("The food reference scriptable object")]
    /// <summary>
    /// The food reference scriptable object
    /// </summary>
    [SerializeField] Food foodReference;

    /// <summary>
    /// The rigidbody of this food game object
    /// </summary>
    private Rigidbody _foodRB;

    private void Awake()
    {
        // On awake, initialize the food rigidbody
        _foodRB = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// Method called to toggle ON / OFF IsKinematic on this food game object (Off = Non-Kinematic RB / ON = Kinematic)
    /// </summary>
    /// <param name="toggle"></param>
    public void ToggleRigidBodyKinematic(bool toggle)
    {
        // If the food rigidbody is null, then initialize it
        if(_foodRB == null)
            _foodRB = GetComponent<Rigidbody>();
        // Toggle the rigidbody to the toggle parameter passed
        _foodRB.isKinematic = toggle;
    }

    /// <summary>
    /// The sprite of this food game object
    /// </summary>
    /// <returns></returns>
    public Sprite FoodSprite()
    {
        return foodReference.FoodSprite();
    }

    /// <summary>
    /// The name of this food game object
    /// </summary>
    /// <returns></returns>
    public string FoodName()
    {
        return foodReference.FoodName();
    }
}
