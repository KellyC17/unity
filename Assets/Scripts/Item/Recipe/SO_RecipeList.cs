using UnityEngine;

[CreateAssetMenu(fileName = "so_RecipeList", menuName = "ScriptableObjects/Recipe/so_RecipeList")]
public class SO_RecipeList : ScriptableObject
{
  [SerializeField]
  public Recipe[] recipes;
}