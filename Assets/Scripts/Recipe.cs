using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Untitled Recipe", menuName = "Recipe System/Recipe")]
public class Recipe : Food
{
    [SerializeField]
    [Tooltip("The name of this recipe")]
    string recipeName;
    [SerializeField]
    [Tooltip("The sprite used to represent this recipe")]
    Sprite recipeSprite;
    [SerializeField]
    [Tooltip("The recipe game object prefab")]
    FoodGameObject recipePrefab;

    [SerializeField]
    List<RecipeIngredientData> recipeIngredients = new List<RecipeIngredientData>();

    /// <summary>
    /// The name of this recipe
    /// </summary>
    public string RecipeName
    {
        get { return recipeName; }
    }

    /// <summary>
    /// The sprite used to represent this recipe
    /// </summary>
    public Sprite RecipeSprite
    {
        get { return recipeSprite; }
    }

    /// <summary>
    /// The recipe game object prefab
    /// </summary>
    public FoodGameObject RecipePrefab
    {
        get { return recipePrefab; }
    }

    /// <summary>
    /// The recipe game object prefab
    /// </summary>
    /// <returns></returns>
    public override FoodGameObject FoodGameObjectPrefab()
    {
        return RecipePrefab;
    }

    /// <summary>
    /// The name of this recipe
    /// </summary>
    /// <returns></returns>
    public override string FoodName()
    {
        return RecipeName;
    }

    /// <summary>
    /// The sprite used to represent this recipe
    /// </summary>
    /// <returns></returns>
    public override Sprite FoodSprite()
    {
        return RecipeSprite;
    }

    /// <summary>
    /// Returns whether or not the given ingredient data list meets the requirements to make this recipe
    /// </summary>
    /// <param name="ingredientData">The ingredient data</param>
    /// <returns></returns>
    public bool CanMakeRecipeFromIngredients(List<RecipeIngredientData> ingredientData)
    {
        // If the recipe ingredients count is not equal to the ingredients data list passed in,
        // then return false
        if (recipeIngredients.Count != ingredientData.Count)
            return false;

        // Iterate through the recipe ingredient requirements
        foreach (RecipeIngredientData recipeIngredient in recipeIngredients)
        {
            // Cache whether or not this ingredient requirement has been met
            bool ingredientRequirementMet = false;

            // Iterate through the ingredients in the ingredient data parameter passed
            foreach (RecipeIngredientData ingredient in ingredientData)
            {
                // If the recipe ingredient and the ingredient are the same as well
                // as their counts, then the recipe requirement have been met for this ingredient
                if(recipeIngredient.recipeIngredient == ingredient.recipeIngredient &&
                    recipeIngredient.ingredientCount == ingredient.ingredientCount)
                {
                    // This ingredient requirement has been met,
                    // break out of this for loop
                    ingredientRequirementMet = true;
                    break;
                }
            }
            // If the ingredient requirement has not been met,
            // Then break out of this for loop
            if (ingredientRequirementMet == false)
                return false;
        }

        return true;
    }
}

[System.Serializable]
/// <summary>
/// Struct used to represent recipe data
/// </summary>
public struct RecipeIngredientData
{
    /// <summary>
    /// The ingredient required for a recipe
    /// </summary>
    [Tooltip("The ingredient reference")]
    public Ingredient recipeIngredient;

    /// <summary>
    /// The ingredient count required for a recipe
    /// </summary>
    [Tooltip("The count of this ingredient required for this recipe")]
    public int ingredientCount;
}

/// <summary>
/// Static helper class for recipe and ingredient functions
/// </summary>
public static class RecipeUtility
{
    /// <summary>
    /// Converts a list of ingredients to a list of recipe ingredient data
    /// </summary>
    /// <param name="ingredients">List of ingredients</param>
    /// <returns></returns>
    public static List<RecipeIngredientData> ConvertIngredientListToRecipeIngredientDataList(List<Ingredient> ingredients)
    {
        /// Create a dictionary of ingredients to counts key value pair
        Dictionary<Ingredient, int> ingredientCountDict = new Dictionary<Ingredient, int>();
        // Iterate through all of the ingredients
        foreach (Ingredient ingredient in ingredients)
        {
            // If the ingredients dictionary contains a key for the given ingredient,
            // Then increment the count value by 1
            if (ingredientCountDict.ContainsKey(ingredient))
            {
                ingredientCountDict[ingredient]++;
            }
            // Otherwise, add the ingredient to the dictionary as a key
            // with a value of 1
            else
            {
                ingredientCountDict.Add(ingredient, 1);
            }
        }

        // Create a list of recipe data
        List<RecipeIngredientData> recipeDataList = new List<RecipeIngredientData>();
        // Iterate through all the key value pairs of the dictionary
        foreach (KeyValuePair<Ingredient,int> ingredientToCount in ingredientCountDict)
        {
            // Create a new recipe ingredient data struct
            // Assign the ingredient key and the ingredient count value
            RecipeIngredientData recipeIngredient = new RecipeIngredientData();
            recipeIngredient.recipeIngredient = ingredientToCount.Key;
            recipeIngredient.ingredientCount = ingredientToCount.Value;
            // Add it to the list
            recipeDataList.Add(recipeIngredient);
        }

        // Finally, return the recipe data list
        return recipeDataList;
    }
}