using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaladUIDisplayer : MonoBehaviour
{
    [SerializeField] List<UnityEngine.UI.Image> spriteImages;
    [SerializeField] CanvasGroup saladUICanvasGroup;


    public void ToggleSaladUI(bool toggle)
    {
        saladUICanvasGroup.alpha = toggle ? 1.0f : 0.0f;
    }

    public void UpdateSaladIngredientsUI(List<Ingredient> ingredients)
    {
        if(ingredients.Count == 0)
        {
            ToggleSaladUI(false);
        }
        else
        {
            for (int i = 0; i < spriteImages.Count; i++)
            {
                if(i < ingredients.Count)
                {
                    spriteImages[i].sprite = ingredients[i].IngredientSprite;
                    spriteImages[i].gameObject.SetActive(true);
                }
                else
                {
                    spriteImages[i].gameObject.SetActive(false);
                }
            }
            ToggleSaladUI(true);
        }
    }
}
