using System.Collections.Generic;
using UnityEngine;

public class Mixer : Tool
{
    [SerializeField]
    public BoxCollider2D mixingTriggerCollider;
    public override bool isContainer => true;

    // Track liquids and solids
    private Dictionary<LiquidContainer, GameObject> activeLiquids = new Dictionary<LiquidContainer, GameObject>();
    private List<GameObject> activeSolids = new List<GameObject>();

    // Receive liquid
    public override void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
    {
        if (activeLiquids.ContainsKey(sourceContainer)) return;

        IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(sourceContainer.ItemId);
        if (details != null)
        {
            Vector3 position = transform.position + details.mixerSnapPosition;
            GameObject newLiquid = Instantiate(liquidPrefab, position, Quaternion.identity);
            LiquidDragHandler dragHandler = newLiquid.GetComponent<LiquidDragHandler>();
            if (dragHandler != null)
            {
                dragHandler.sourceContainer = sourceContainer;
                dragHandler.mixer = this;
            }
            activeLiquids.Add(sourceContainer, newLiquid);
        }
    }

    // Receive solid
    public void ReceiveSolid(GameObject solidPrefab)
    {
        IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(solidPrefab.GetComponent<Ingredient>().ItemId);
        if (details != null)
        {
            Vector3 position = transform.position + details.mixerSnapPosition;
            GameObject newSolid = Instantiate(solidPrefab, position, Quaternion.identity);
            activeSolids.Add(newSolid);
        }
    }

    // Remove liquid
    public void OnLiquidDraggedOut(LiquidContainer sourceContainer)
    {
        if (activeLiquids.ContainsKey(sourceContainer))
        {
            Debug.Log("Remove liquid");
            activeLiquids.Remove(sourceContainer);
        }
    }

    // Remove solid
    public void OnSolidDraggedOut(GameObject solidObject)
    {
        if (activeSolids.Contains(solidObject))
        {
            Debug.Log("Remove solid object");
            activeSolids.Remove(solidObject);
        }
    }

    // Apply mixing action
    public override void ApplyToolAction()
    {
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        Collider2D[] overlaps = new Collider2D[10];
        int count = mixingTriggerCollider.Overlap(contactFilter, overlaps);

        Dictionary<string, int> ingredients = new Dictionary<string, int>();

        for (int i = 0; i < count; i++)
        {
            Ingredient ingredient = overlaps[i].gameObject.GetComponent<Ingredient>();
            if (ingredient != null)
            {
                string ingredientId = ingredient.ItemId;
                ingredients[ingredientId] = ingredients.ContainsKey(ingredientId) ? ingredients[ingredientId] + 1 : 1;
            }
        }

        (Recipe recipe, int output) = InventoryManager.Instance.GetMatchingRecipe(ItemId, ingredients);

        if (recipe != null)
        {
            for (int i = 0; i < count; i++)
            {
                Destroy(overlaps[i].gameObject);
            }
            activeLiquids.Clear();
            activeSolids.Clear();

            Debug.Log("OutputId: " + recipe.outputId);
            InstantiateOutput(recipe.outputPrefab);
        }
    }

    // Instantiate output
    public void InstantiateOutput(GameObject prefab)
    {
        Vector3 position = transform.position - new Vector3(0.1f, 0f, 0f);
        Instantiate(prefab, position, Quaternion.identity);
    }

    private bool IsMouseOver()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return mixingTriggerCollider.OverlapPoint(mousePosition);
    }
}