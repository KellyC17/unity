using UnityEngine;

public class Chopboard : MonoBehaviour
{
  public void ChopItem(GameObject item)
  {
    // For demo purposes, let's just destroy the item and create a chopped version.
    Destroy(item); // Remove the original item
  }
}
