using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    private IngredientDetails _ingredientDetails;

    public HashSet<ToolType> isOnTool { get; private set; } = new HashSet<ToolType>();

    public HashSet<ToolEffect> isReadyForTool { get; private set; } = new HashSet<ToolEffect>();

    public IngredientDetails IngredientDetails
    {
        get { return _ingredientDetails; }
        set { _ingredientDetails = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Tool>() != null)
        {
            isOnTool.Add(other.GetComponent<Tool>().ToolDetails.ToolType);
            isReadyForTool.Add(other.GetComponent<Tool>().ToolDetails.ToolEffect);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Tool>() != null)
        {
            isOnTool.Remove(other.GetComponent<Tool>().ToolDetails.ToolType);
            isReadyForTool.Remove(other.GetComponent<Tool>().ToolDetails.ToolEffect);
        }
    }

    public bool IsWithTool(ToolType tool)
    {
        return isOnTool.Contains(tool);
    }

    public bool IsReadyForTool(ToolEffect toolEffect)
    {
        return isReadyForTool.Contains(toolEffect);
    }

}
