using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladBowl : Interactable
{
    [SerializeField] int maxIngredients = 4;
    [SerializeField] MeshRenderer saladMesh;
    List<Ingredient> saladIngredients = new List<Ingredient>();
    private void Awake()
    {
        UpdateSaladRenderer();
    }

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
                AddSaladIngredientToList(droppedFood);
            }
        }
    }

    private void AddSaladIngredientToList(FoodGameObject foodAdded)
    {
        saladIngredients.Add(foodAdded.FoodIngredient);
        Destroy(foodAdded.gameObject);
        UpdateSaladRenderer();
        OnSaladIngredientsChanged.Invoke(this);
    }


    public void ConsumeBowl()
    {
        saladIngredients.Clear();
        UpdateSaladRenderer();
        OnSaladIngredientsChanged.Invoke(this);
    }

    private bool MaxIngredientsHeld()
    {
        return saladIngredients.Count >= maxIngredients;
    }

    private void UpdateSaladRenderer()
    {
        saladMesh.gameObject.SetActive(saladIngredients.Count > 0);
    }

    public SaladBowlChange OnSaladIngredientsChanged;
}

[System.Serializable]
public class SaladBowlChange : UnityEngine.Events.UnityEvent<SaladBowl>
{

}
