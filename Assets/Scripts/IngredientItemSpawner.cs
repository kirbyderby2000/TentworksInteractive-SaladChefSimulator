using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for ingredient spawners
/// </summary>
public class IngredientItemSpawner : HoldableCoordinator
{
    [SerializeField] Ingredient ingredientToSpawn;

    [SerializeField] Transform uiReferenceTransform;

    /// <summary>
    /// Returns the the holdable item component of an ingredient spawned
    /// </summary>
    /// <returns></returns>
    public override HoldableItem GetHoldableItem()
    {
        // Instantiate the food referent prefab
        FoodGameObject instantiatedFood = Instantiate(ingredientToSpawn.IngredientPrefab, transform.position, ingredientToSpawn.IngredientPrefab.transform.rotation, transform.parent);
        // Return the holdable item component
        return instantiatedFood.GetHoldableItemComponent();
    }

    /// <summary>
    /// Returns the ingredient sprite
    /// </summary>
    /// <returns></returns>
    public Sprite GetIngredientSprite()
    {
        return ingredientToSpawn.IngredientSprite;
    }

    /// <summary>
    /// Returns the position where UI should be indicated for this ingredient spawner
    /// </summary>
    /// <returns></returns>
    public Transform GetUITransformReference()
    {
        return uiReferenceTransform;
    }
}
