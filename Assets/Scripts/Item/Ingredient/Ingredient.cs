using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    private IngredientDetails _ingredientDetails;

    public bool isReadyToChop { get; private set; } = false;

    public IngredientDetails IngredientDetails
    {
        get { return _ingredientDetails; }
        set { _ingredientDetails = value; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Tool tool = other.GetComponent<Tool>();
        if (tool?.ToolDetails?.ToolType == ToolType.chopBoard)
        {
            isReadyToChop = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other == null)
        {
            Debug.LogError("OnTriggerExit2D: other is null");
            return;
        }

        Tool tool = other.GetComponent<Tool>();
        if (tool != null && tool.ToolDetails != null && tool.ToolDetails.ToolType == ToolType.chopBoard)
        {
            isReadyToChop = false;
        }
    }
}
