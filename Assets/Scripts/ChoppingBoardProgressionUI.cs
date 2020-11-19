using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChoppingBoardProgressionUI : MonoBehaviour
{
    [Header("UI Game Object References")]
    [SerializeField] CanvasGroup choppingBoardUICanvasGroup;

    [SerializeField] UnityEngine.UI.Image startIngredientImage;

    [SerializeField] UnityEngine.UI.Image resultIngredientImage;


    [SerializeField] UnityEngine.UI.Slider choppingBoardProgressionSlider;

    [Header("The Chopping Board to Provide UI to")]
    [SerializeField] ChoppingBoard choppingBoard;


    private void Awake()
    {
        ToggleChoppingBoard(false);
        choppingBoard.OnChoppableItemBoarded.AddListener(OnChoppableItemBoarded);
        choppingBoard.OnChoppableItemUnboarded.AddListener(OnChoppableItemUnboarded);
        PositionChoppingBoard();
    }


    private void PositionChoppingBoard()
    {
        transform.position = MainCamera.CameraInstance.WorldToScreenPoint(choppingBoard.transform.position);
    }

    private void OnChoppableItemBoarded(Choppable choppableItem)
    {
        ChoppableItemProgressionChanged(choppableItem);
        startIngredientImage.sprite = choppableItem.GetComponent<FoodGameObject>().FoodIngredient.IngredientSprite;
        resultIngredientImage.sprite = choppableItem.ChoppedIngredient.IngredientSprite;
        choppableItem.OnChoppedProgressionChanged.AddListener(ChoppableItemProgressionChanged);
        ToggleChoppingBoard(true);
    }

    private void OnChoppableItemUnboarded(Choppable choppableItem)
    {
        choppableItem.OnChoppedProgressionChanged.RemoveListener(ChoppableItemProgressionChanged);
        ToggleChoppingBoard(false);
    }

    private void ChoppableItemProgressionChanged(Choppable choppableItem)
    {
        OnChoppedItemProgressionChanged(choppableItem.ChoppedAmount);
    }


    private void OnChoppedItemProgressionChanged(float progression)
    {
        choppingBoardProgressionSlider.value = progression;
    }

    private void ToggleChoppingBoard(bool toggle)
    {
        choppingBoardUICanvasGroup.alpha = toggle ? 1.0f : 0.0f;
    }


}
