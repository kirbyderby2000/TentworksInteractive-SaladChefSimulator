using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for food game objects
/// </summary>
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(HoldableItem))]
public class FoodGameObject : MonoBehaviour
{
    
    [Tooltip("The food reference scriptable object")]
    /// <summary>
    /// The food reference scriptable object
    /// </summary>
    [SerializeField] Ingredient foodReference;

    HoldableItem _holdableItemComponent;

    /// <summary>
    /// The ingredient of this food
    /// </summary>
    public Ingredient FoodIngredient
    {
        get { return foodReference; }
    }


    private void Awake()
    {
        _holdableItemComponent = GetComponent<HoldableItem>();
    }


    /// <summary>
    /// Gets the holdable item component of this food item
    /// </summary>
    /// <returns></returns>
    public HoldableItem GetHoldableItemComponent()
    {
        if(_holdableItemComponent == null)
            _holdableItemComponent = GetComponent<HoldableItem>();
        return _holdableItemComponent;
    }
}
