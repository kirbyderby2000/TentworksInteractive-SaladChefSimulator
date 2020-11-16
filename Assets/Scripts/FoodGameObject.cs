using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for food game objects
/// </summary>
[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshCollider))]
public class FoodGameObject : MonoBehaviour
{
    [Tooltip("The food reference scriptable object")]
    /// <summary>
    /// The food reference scriptable object
    /// </summary>
    [SerializeField] Food foodReference;


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
