using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class used for ingredient spawners
/// </summary>
public class IngredientItemSpawner : HoldableCoordinator
{
    [SerializeField] Ingredient ingredientToSpawn;

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
}
