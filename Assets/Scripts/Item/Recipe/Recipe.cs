using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Recipe
{
  public string recipeId;
  public string recipeName;

  [ItemIdDescription]
  public string outputId; // 最终生成的物品（如芒果奶昔）

  public GameObject outputPrefab; // 最终生成的物品的预制体

  public int outputAmount; // 最终生成的物品数量

  [ItemIdDescription]
  public string[] inputIngredientIds;

  public int[] inputIngredientAmounts; // 每种食材的数量

  [ItemIdDescription]
  public string toolId;

  internal int MatchesIngredients(string toolId, Dictionary<string, int> ingredients)
  {
    Debug.Log("Checking recipe " + recipeId);
    if (this.toolId != toolId)
    {
      return 0;
    }
    Debug.Log(1);

    int multiple = -1;

    // Determine the multiple
    foreach (KeyValuePair<string, int> ingredient in ingredients)
    {
      Debug.Log(2);

      bool found = false;
      for (int i = 0; i < inputIngredientIds.Length; i++)
      {
        if (ingredient.Key == inputIngredientIds[i])
        {
          if (ingredient.Value % inputIngredientAmounts[i] != 0)
          {
            Debug.Log(3);

            return 0;
          }
          int currentMultiple = ingredient.Value / inputIngredientAmounts[i];
          if (multiple == -1)
          {
            multiple = currentMultiple;
          }
          else if (multiple != currentMultiple)
          {
            Debug.Log("Checking recipe " + recipeId + " for tool " + toolId);
            return 0;
          }
          found = true;
          break;
        }
      }
      if (!found)
      {
        Debug.Log(4);
        return 0;
      }
    }

    // Ensure all recipe ingredients appear in the same multiple
    for (int i = 0; i < inputIngredientIds.Length; i++)
    {
      if (!ingredients.ContainsKey(inputIngredientIds[i]) || ingredients[inputIngredientIds[i]] / inputIngredientAmounts[i] != multiple)
      {
        Debug.Log(5);
        return 0;
      }
    }

    Debug.Log("Recipe " + recipeId + " matches ingredients" + " with multiple " + multiple);

    return multiple;
  }
}