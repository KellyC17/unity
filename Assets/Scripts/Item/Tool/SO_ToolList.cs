using UnityEngine;

[CreateAssetMenu(fileName = "so_ToolList", menuName = "ScriptableObjects/Tool/ToolList")]
public class SO_ToolList : ScriptableObject
{
  [SerializeField]
  public ToolDetails[] toolDetails;
}