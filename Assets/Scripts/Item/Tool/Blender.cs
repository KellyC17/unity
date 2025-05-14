using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
  [SerializeField]
  public BoxCollider2D blendingTriggerCollider;
  public override bool isContainer => true;

  // Add liquid tracking
  private Dictionary<LiquidContainer, GameObject> activeLiquids = new Dictionary<LiquidContainer, GameObject>();

  // Add new method to handle receiving liquid
  public override void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
  {
    // Don't accept liquid from the same source
    if (activeLiquids.ContainsKey(sourceContainer)) return;

    // Instantiate new liquid
    var position = transform.position - new Vector3(0.1f, 0f, 0f);
    GameObject newLiquid = Instantiate(liquidPrefab, position, Quaternion.identity);
    activeLiquids.Add(sourceContainer, newLiquid);
  }

  public override void Update()
  {
    if (Input.GetMouseButtonDown(0) && IsMouseOver())  // Left click and mouse is over the object
    {
      ApplyToolAction();
    }
  }
  public override void ApplyToolAction()
  {
    ContactFilter2D contactFilter = new ContactFilter2D();
    contactFilter.useTriggers = true;
    int count = ingredientCollider.Overlap(contactFilter, overlaps);

    Dictionary<string, int> ingredients = new Dictionary<string, int>();

    for (int i = 0; i < count; i++)
    {
      string ingredientId = overlaps[i].gameObject.GetComponent<Ingredient>().ItemId;
      ingredients[ingredientId] = ingredients.ContainsKey(ingredientId) ? ingredients[ingredientId] + 1 : 1;
    }

    (Recipe recipe, int output) = InventoryManager.Instance.GetMatchingRecipe(ItemId, ingredients);

    if (recipe != null)
    {
      for (int i = 0; i < count; i++)
      {
        Destroy(overlaps[i].gameObject);
      }
      InstantiateLiquid(recipe.outputPrefab);

      ingredients.Clear();
    }
  }

  public override void InstantiateLiquid(GameObject prefab)
  {
    // this position is specific to the blender image
    var position = transform.position - new Vector3(0.1f, 0f, 0f);
    Instantiate(prefab, position, Quaternion.identity);
  }

  private bool IsMouseOver()
  {
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    return blendingTriggerCollider.OverlapPoint(mousePosition);
  }
}
