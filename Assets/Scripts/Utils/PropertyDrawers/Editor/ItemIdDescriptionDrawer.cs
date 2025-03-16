using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ItemIdDescriptionAttribute))]
public class ItemIdDescriptionDrawer : PropertyDrawer
{
  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    return EditorGUI.GetPropertyHeight(property) * 2;
  }

  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    EditorGUI.BeginProperty(position, label, property);

    if (property.propertyType == SerializedPropertyType.String)
    {
      EditorGUI.BeginChangeCheck();
      string itemId = EditorGUI.TextField(new Rect(position.x, position.y, position.width, position.height / 2), label, property.stringValue);

      EditorGUI.LabelField(new Rect(position.x, position.y + position.height / 2, position.width, position.height / 2), "Item Name: " + GetItemName(property.stringValue));

      if (EditorGUI.EndChangeCheck())
      {
        property.stringValue = itemId;
      }
    }


    EditorGUI.EndProperty();
  }

  private string GetItemName(string itemId)
  {
    SO_ToolList toolList = (SO_ToolList)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Tool/so_ToolList.asset", typeof(SO_ToolList));
    foreach (ToolDetails toolDetails in toolList.toolDetails)
    {
      if (toolDetails.itemId == itemId)
      {
        return toolDetails.name;
      }
    }

    SO_IngredientList ingredientList = (SO_IngredientList)AssetDatabase.LoadAssetAtPath("Assets/ScriptableObjects/Ingredient/so_IngredientList.asset", typeof(SO_IngredientList));
    foreach (IngredientDetails ingredientDetails in ingredientList.ingredientDetails)
    {
      if (ingredientDetails.itemId == itemId)
      {
        return ingredientDetails.name;
      }
    }

    return string.Empty;
  }
}