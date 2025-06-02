using System.Collections.Generic;
using UnityEngine;

public class Blender : Tool
{
  [SerializeField]
  public BoxCollider2D blendingTriggerCollider;
  public override bool isContainer => true;

  // Add liquid tracking
  private Dictionary<LiquidContainer, GameObject> activeLiquids = new Dictionary<LiquidContainer, GameObject>();

  // Add solid tracking
  private List<GameObject> activeSolids = new List<GameObject>();

  // Add new method to handle receiving liquid
  public override void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
  {
    // Don't accept liquid from the same source
    if (activeLiquids.ContainsKey(sourceContainer)) return;

    // Get the ingredient details to use the correct snap position
    IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(sourceContainer.ItemId);
    if (details != null)
    {
      // Instantiate new liquid at the snap position
      Vector3 position = transform.position + details.blenderSnapPosition;
      GameObject newLiquid = Instantiate(liquidPrefab, position, Quaternion.identity);
      LiquidDragHandler dragHandler = newLiquid.GetComponent<LiquidDragHandler>();
      if (dragHandler != null)
      {
        dragHandler.sourceContainer = sourceContainer;
        dragHandler.blender = this;
      }
      activeLiquids.Add(sourceContainer, newLiquid);
    }
  }

  public void ReceiveSolid(GameObject solidPrefab)
{
    // Get the ingredient details to use the correct snap position
    IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(solidPrefab.GetComponent<Ingredient>().ItemId);
    if (details != null)
    {
        // Instantiate new solid at the snap position
        Vector3 position = transform.position + details.blenderSnapPosition;
        GameObject newSolid = Instantiate(solidPrefab, position, Quaternion.identity);
        activeSolids.Add(newSolid);
    }
}

  public void OnLiquidDraggedOut(LiquidContainer sourceContainer)
  {
    if (activeLiquids.ContainsKey(sourceContainer))
    {
      // Just remove reference, do NOT destroy the GameObject
      Debug.Log("Remove liquid");
      activeLiquids.Remove(sourceContainer);
    }
  }

public void OnSolidDraggedOut(GameObject solidObject)
{
    if (activeSolids.Contains(solidObject))
    {
        Debug.Log("Remove solid object");
        activeSolids.Remove(solidObject);
    }
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
