using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladBowl : Interactable
{
    [SerializeField] int maxIngredients = 4;
    [SerializeField] List<Ingredient> saladIngredients = new List<Ingredient>();

    public override void PlayerDroppedItem(HoldableItem droppedItem, PlayerController playerThatDroppedTheItem)
    {
        // If the max ingredients are held in this bowl, then do nothing
        if (MaxIngredientsHeld())
            return;

        // Try and get the dropped item's food game object component
        FoodGameObject droppedFood = droppedItem.GetComponent<FoodGameObject>();
        if(droppedFood != null)
        {
            // If the dropped food is bowlable, then add the salad ingredient
            if (droppedFood.FoodIngredient.IsBowlable)
            {

            }
        }
    }

    private void AddSaladIngredientToList(FoodGameObject foodAdded)
    {

    }

    private bool MaxIngredientsHeld()
    {
        return saladIngredients.Count >= maxIngredients;
    }
}
