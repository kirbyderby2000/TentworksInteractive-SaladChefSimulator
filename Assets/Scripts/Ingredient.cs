using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
public class Ingredient : MonoBehaviour
{
    [Tooltip("The sprite used to represent this ingredient")]
    [SerializeField] Sprite ingredientSprite;

    /// <summary>
    /// The rigidbody used for this ingredient
    /// </summary>
    Rigidbody _ingredientRB;

    private void Awake()
    {
        // Initialize this ingredient's rigid body
        _ingredientRB = GetComponent<Rigidbody>();
    }

    /// <summary>
    /// The sprite used to represent this ingredient
    /// </summary>
    public Sprite IngredientSprite
    {
        get { return ingredientSprite; }
    }

    /// <summary>
    /// Method called to toggle ON / OFF IsKinematic on this ingredient (Off = Non-Kinematic RB / ON = Kinematic)
    /// </summary>
    /// <param name="toggle">Whether or not this rigid body </param>
    public void ToggleRigidBodyKinematic(bool toggle)
    {
        if(_ingredientRB == null)
        {
            _ingredientRB = GetComponent<Rigidbody>();
        }
        _ingredientRB.isKinematic = toggle;
    }
}
