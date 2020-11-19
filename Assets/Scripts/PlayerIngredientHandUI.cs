using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIngredientHandUI : MonoBehaviour
{
    [SerializeField] PlayerController playerToDisplay;

    [SerializeField] IngredientDisplayer ingredientDisplayer;

    private void Awake()
    {
        playerToDisplay.PlayerHand.OnHeldItemsChanged.AddListener(PlayerHandChanged);
        PlayerHandChanged(playerToDisplay.PlayerHand);
    }


    private void Update()
    {
        UpdateUIPosition();
    }


    private void UpdateUIPosition()
    {
        transform.position = MainCamera.CameraInstance.WorldToScreenPoint(playerToDisplay.transform.position);
    }

    private void PlayerHandChanged(PlayerHandManager playersNewHand)
    {
        List<Ingredient> ingredientsInHand = new List<Ingredient>();
        FoodGameObject foodGameObjectAttached;

        foreach (var item in playersNewHand.ItemsInHand)
        {
            foodGameObjectAttached = item.GetComponent<FoodGameObject>();
            if(foodGameObjectAttached != null)
            {
                ingredientsInHand.Add(foodGameObjectAttached.FoodIngredient);
            }
        }

        ingredientDisplayer.UpdateSaladIngredientsUI(ingredientsInHand);

        UpdateUIPosition();

        this.gameObject.SetActive(ingredientsInHand.Count > 0);
    }
}
