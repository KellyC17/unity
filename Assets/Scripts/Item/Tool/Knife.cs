
using UnityEngine;

public class Knife : Tool
{
  // void ApplyToolAction()
  // {
  //   var (recipe, output) = InventoryManager.Instance.GetMatchingRecipe(this.ItemId, ingredients);
  //   // instantiate recipe's output id's matching prefab
  //   if (recipe != null)
  //   {
  //     Debug.Log("Recipe: " + recipe.recipeName);

  //     InstantiateInTheMiddle(collidingObjects, recipe.outputPrefab);

  //     //todo: refactor
  //     List<GameObject> objectsToDestroy = new List<GameObject>(collidingObjects);
  //     foreach (var obj in objectsToDestroy)
  //     {
  //       Destroy(obj);
  //     }
  //     ingredients.Clear();
  //     collidingObjects.Clear();
  //   }
  // }

  public override void ApplyToolAction()
  {
    Collider2D myCollider = GetComponent<BoxCollider2D>();
    Collider2D[] overlaps = new Collider2D[10]; // Adjust size as needed
    int count = myCollider.Overlap(new ContactFilter2D(), overlaps);
    Debug.Log("Overlapping with " + count + " objects");

    for (int i = 0; i < count; i++)
    {
      Debug.Log("Overlapping with: " + overlaps[i].gameObject.name);
      Ingredient ingredient = overlaps[i].gameObject.GetComponent<Ingredient>();
      if (ingredient != null)
      {
        Debug.Log("Chopping " + ingredient);
        IngredientDetails ingredientDetails = InventoryManager.Instance.GetIngredientDetails(ingredient.ItemId);
        if (!ingredientDetails.canBeChopped)
        {
          // todo: error
          continue;
        }
        Vector3 position = ingredient.transform.position;
        Destroy(ingredient.gameObject);
        Instantiate(ingredientDetails.choppedPrefab, position, Quaternion.identity);
      }
    }
  }
}
