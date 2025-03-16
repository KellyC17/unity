using UnityEngine;

public class Chopboard : MonoBehaviour
{
  public void ChopItem(GameObject item)
  {
    // Here, you can add the logic for chopping the item.
    // For example, instantiate the chopped pieces or change the item's appearance.
    Debug.Log(item.name + " is being chopped!");

    // For demo purposes, let's just destroy the item and create a chopped version.
    Destroy(item); // Remove the original item
  }
}
