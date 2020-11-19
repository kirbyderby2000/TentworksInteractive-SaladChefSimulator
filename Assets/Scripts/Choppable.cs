using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component attached to choppable ingredients
/// </summary>
public class Choppable : MonoBehaviour
{
    /// <summary>
    /// How long it takes to completely chop this ingredient
    /// </summary>
    [SerializeField] float choppingDuration = 5.0f;

    /// <summary>
    /// The resulting ingredient once this item is chopped
    /// </summary>
    [SerializeField] Ingredient resultingChoppedIngredient;

    /// <summary>
    /// The time spent chopping this ingredient
    /// </summary>
    private float _choppedTime = 0.0f;

    /// <summary>
    /// The amount spent chopping this 
    /// </summary>
    public float ChoppedAmount
    {
        get
        {
            return Mathf.Clamp01(_choppedTime / choppingDuration);
        }
    }

    /// <summary>
    /// Whether or not this ingredient is completely chopped
    /// </summary>
    public bool ChopComplete
    {
        get
        {
            return _choppedTime >= choppingDuration;
        }
    }

    /// <summary>
    /// Returns the resulting ingredient when this item is chopped
    /// </summary>
    public Ingredient ChoppedIngredient
    {
        get
        {
            return resultingChoppedIngredient;
        }
    }

    /// <summary>
    /// Increments the chopped time to this ingredient
    /// </summary>
    /// <param name="timeChopped"></param>
    public void AddChoppedTime(float timeChopped)
    {
        _choppedTime += timeChopped;
        OnChoppedProgressionChanged.Invoke(this);
    }

    public ChoppableEventHandler OnChoppedProgressionChanged;
}

[System.Serializable]
public class ChoppableEventHandler : UnityEngine.Events.UnityEvent<Choppable>
{

}


