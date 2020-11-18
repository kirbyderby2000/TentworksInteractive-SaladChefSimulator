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
    [SerializeField] Food foodReference;

    HoldableItem _holdableItemComponent;

    private void Awake()
    {
        _holdableItemComponent = GetComponent<HoldableItem>();
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
