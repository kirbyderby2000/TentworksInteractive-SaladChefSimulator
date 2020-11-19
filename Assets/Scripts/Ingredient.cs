using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Untitled Ingredient", menuName ="Recipe System/Ingredient")]
public class Ingredient : ScriptableObject
{
    [Tooltip("The name of this ingredient")]
    [SerializeField] string ingredientName;
    [Tooltip("The sprite used to represent this ingredient")]
    [SerializeField] Sprite ingredientSprite;
    [Tooltip("The ingredient game object prefab")]
    [SerializeField] FoodGameObject ingredientPrefab;
    [Tooltip("Whether or not this ingredient is bowlable")]
    [SerializeField] bool isBowlable = false;

    /// <summary>
    /// The sprite used to represent this ingredient
    /// </summary>
    public Sprite IngredientSprite
    {
        get { return ingredientSprite; }
    }

    /// <summary>
    /// The name of the ingredient
    /// </summary>
    public string IngredientName
    {
        get { return ingredientName; }
    }

    /// <summary>
    /// The ingredient game object prefab
    /// </summary>
    public FoodGameObject IngredientPrefab
    {
        get { return ingredientPrefab; }
    }

    /// <summary>
    /// Whether or not this ingredient can be placed in a bowl
    /// </summary>
    public bool IsBowlable
    {
        get { return isBowlable; }
    }
    
}
