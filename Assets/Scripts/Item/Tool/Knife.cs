
using UnityEngine;

public class Knife : Tool
{
  
  void Update()
  {
    base.Update();
    if (overlapCount > 0)
    {
      ApplyToolAction();
    }
  }

  public override void ApplyToolAction()
  {
    overlaps = new Collider2D[10]; // Adjust size as needed
    int count = ingredientCollider.Overlap(new ContactFilter2D(), overlaps);
    Debug.Log("Overlapping with " + count + " objects");

    for (int i = 0; i < count; i++)
    {
      Debug.Log("Overlapping with: " + overlaps[i].gameObject.name);
      Ingredient ingredient = overlaps[i].gameObject.GetComponent<Ingredient>();
      if (ingredient != null && ingredient.isReadyToChop)
      {
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
