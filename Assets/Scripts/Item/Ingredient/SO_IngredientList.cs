using UnityEngine;

[CreateAssetMenu(fileName = "so_IngredientList", menuName = "ScriptableObjects/Ingredient/so_IngredientList")]
public class SO_IngredientList : ScriptableObject
{
  [SerializeField]
  public IngredientDetails[] ingredientDetails;
}