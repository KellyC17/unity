using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Steamer : Tool
{
  [SerializeField]
  public BoxCollider2D SteamTriggerCollider;
  public override bool isContainer => true;
  private bool isSteaming = false;
  public ProgressBar progressBar;

  public override void Update()
  {
    if (Input.GetMouseButtonDown(0) && IsMouseOver())  // Left click and mouse is over the object
    {
      if (!isSteaming)
      {
        ApplyToolAction();
      }
    }
  }

  public override void ApplyToolAction()
  {
    // Only start steaming if we have any liquids
    if (activeLiquids.Count == 0) return;

    progressBar.maxValue = 60;
    progressBar.currentValue = 0;
    StartCoroutine(UpdateProgress(progressBar));
  }

  // New method to handle receiving liquid
  public override void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
  {
    // Don't accept liquid from the same source
    if (activeLiquids.ContainsKey(sourceContainer)) return;

    // Get the ingredient details to use the correct snap position
    IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(sourceContainer.ItemId);
    if (details != null)
    {
      // Instantiate new liquid at the snap position
      Vector3 position = transform.position + details.steamerSnapPosition;
      GameObject newLiquid = Instantiate(liquidPrefab, position, Quaternion.identity);
      LiquidDragHandler dragHandler = newLiquid.GetComponent<LiquidDragHandler>();
      if (dragHandler != null)
      {
          dragHandler.sourceContainer = sourceContainer;
          dragHandler.steamer = this;
      }
      activeLiquids.Add(sourceContainer, newLiquid);
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

  private bool IsMouseOver()
  {
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    return SteamTriggerCollider.OverlapPoint(mousePosition);
  }

  private IEnumerator UpdateProgress(ProgressBar progressBar)
  {
    Debug.Log("Updating progress");
    isSteaming = true;
    float duration = 1f;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      progressBar.currentValue = Mathf.Lerp(0f, progressBar.maxValue, elapsedTime / duration);
      yield return null;
    }

    // When progress is complete
    isSteaming = false;

    // Check for overlapping ingredients
    ContactFilter2D contactFilter = new ContactFilter2D();
    contactFilter.useTriggers = true;
    Collider2D[] overlaps = new Collider2D[10];
    int count = SteamTriggerCollider.Overlap(contactFilter, overlaps);

    Dictionary<string, int> ingredients = new Dictionary<string, int>();

    for (int i = 0; i < count; i++)
    {
      string ingredientId = overlaps[i].gameObject.GetComponent<Ingredient>().ItemId;
      ingredients[ingredientId] = ingredients.ContainsKey(ingredientId) ? ingredients[ingredientId] + 1 : 1;
    }

    Debug.Log("Ingredients: " + string.Join(", ", ingredients));
    Debug.Log("ItemId: " + ItemId);

    // Find matching recipe
    (Recipe recipe, int output) = InventoryManager.Instance.GetMatchingRecipe(ItemId, ingredients);

    if (recipe != null)
    {
      for (int i = 0; i < count; i++)
      {
        Destroy(overlaps[i].gameObject);
      }
      activeLiquids.Clear();

      Debug.Log("OutputId: " + recipe.outputId);
      // Instantiate output
      InstantiateLiquid(recipe.outputPrefab);
    }

    Debug.Log("Steaming process completed!");
  }

  public override void InstantiateLiquid(GameObject prefab)
  {
    // this position is specific to the blender image
    var position = transform.position - new Vector3(0.1f, 0f, 0f);
    Instantiate(prefab, position, Quaternion.identity);
  }
}
