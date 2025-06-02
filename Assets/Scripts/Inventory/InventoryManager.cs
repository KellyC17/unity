using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : SingletonMonoBehaviour<InventoryManager>
{

  private Dictionary<string, ToolDetails> inventory = new Dictionary<string, ToolDetails>();

  private Dictionary<string, IngredientDetails> ingredient = new Dictionary<string, IngredientDetails>();

  private Dictionary<string, Recipe> recipes = new Dictionary<string, Recipe>();


  [SerializeField] private SO_ToolList toolList;

  [SerializeField] private SO_IngredientList ingredientList;

  [SerializeField] private SO_RecipeList recipeList;


  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {
    inventory = new Dictionary<string, ToolDetails>();
    CreateToolDetailsDictionary();
    CreateIngredientDetailsDictionary();
    CreateRecipeDictionary();
  }

  void CreateToolDetailsDictionary()
  {
    foreach (ToolDetails toolDetails in toolList.toolDetails)
    {
      inventory.Add(toolDetails.itemId, toolDetails);
    }
  }

  public ToolDetails GetToolDetails(string id)
  {
    ToolDetails toolDetails;
    inventory.TryGetValue(id, out toolDetails);
    return toolDetails;
  }

  public void CreateIngredientDetailsDictionary()
  {
    foreach (IngredientDetails i in ingredientList.ingredientDetails)
    {
      ingredient.Add(i.itemId, i);
    }
  }

  public IngredientDetails GetIngredientDetails(string id)
  {
    IngredientDetails ingredientDetails;
    ingredient.TryGetValue(id, out ingredientDetails);
    return ingredientDetails;
  }

  public void CreateRecipeDictionary()
  {
    foreach (Recipe r in recipeList.recipes)
    {
      recipes.Add(r.recipeId, r);
    }
  }

  public Recipe GetRecipe(string id)
  {
    Recipe recipe;
    recipes.TryGetValue(id, out recipe);
    return recipe;
  }

  public (Recipe recipe, int output) GetMatchingRecipe(string toolId, Dictionary<string, int> ingredients)
  {
    foreach (Recipe recipe in recipes.Values)
    {
      int output = recipe.MatchesIngredients(toolId, ingredients);
      if (output > 0)
      {
        return (recipe, output);
      }
    }
    return (null, 0);
  }
}