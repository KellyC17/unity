
using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
  public override void ApplyToolAction()
  {
    Collider2D myCollider = GetComponent<BoxCollider2D>();
    Collider2D[] overlaps = new Collider2D[10]; // Adjust size as needed
    int count = myCollider.Overlap(new ContactFilter2D(), overlaps);
    Debug.Log("Overlapping with " + count + " objects");

    Dictionary<string, int> ingredients = new Dictionary<string, int>();

    for (int i = 0; i < count; i++)
    {
      string ingredientId = overlaps[i].gameObject.GetComponent<Ingredient>().ItemId;
      ingredients[ingredientId] = ingredients.ContainsKey(ingredientId) ? ingredients[ingredientId] + 1 : 1;
    }

    // log current state of ingredients
    foreach (KeyValuePair<string, int> ingredient in ingredients)
    {

      Debug.Log(ingredient.Key + ": " + ingredient);
    }

    (Recipe recipe, int output) = InventoryManager.Instance.GetMatchingRecipe(ItemId, ingredients);

    if (recipe != null)
    {
      Debug.Log("Recipe: " + recipe.recipeId);
      Debug.Log("Output: " + output);
      for (int i = 0; i < count; i++)
      {
        Destroy(overlaps[i].gameObject);
      }

      var position = transform.position - new Vector3(0.1f, 0f, 0f);
      Instantiate(recipe.outputPrefab, position, Quaternion.identity);

      ingredients.Clear();
    }
  }
}
