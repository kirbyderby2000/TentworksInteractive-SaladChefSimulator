using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base abstract scriptable object class used to identify food
/// </summary>
public abstract class Food : ScriptableObject
{
    /// <summary>
    /// Returns the sprite of this food
    /// </summary>
    /// <returns></returns>
    public abstract Sprite FoodSprite();

    /// <summary>
    /// Returns the food game object of this food
    /// </summary>
    /// <returns></returns>
    public abstract FoodGameObject FoodGameObjectPrefab();

    /// <summary>
    /// Returns the name of this food
    /// </summary>
    /// <returns></returns>
    public abstract string FoodName();
}
