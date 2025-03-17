using System.Linq;
using UnityEngine;

public class LiquidContainer : Ingredient
{
  [Header("Pouring Settings")]
  [SerializeField] private float pourDistance = 2f; // Maximum distance to detect containers
  [SerializeField] private float pouringSpeed = 1f; // How fast the milk pours
  [SerializeField] private Transform pourPoint; // Point where milk comes out

  [Header("Rotation Settings")]
  [SerializeField] private float rotationSpeed = 5f; // Speed of rotation animation
  [SerializeField] private Vector3 pouringRotation = new Vector3(90f, 0f, 0f); // Target rotation when pouring
  private Quaternion startRotation; // Initial rotation
  private Quaternion targetRotation; // Current target rotation

  private bool isPouring = false;
  private bool canPour = false;
  private Tool nearbyContainer;

  [Header("Trigger Pour related")]
  public Collider2D containerCollider;

  private void Start()
  {

    startRotation = transform.rotation;
    targetRotation = startRotation;
  }

  private void Update()
  {
    // Check for nearby containers
    CheckForContainers();

    // Handle pouring input
    if (canPour && Input.GetKeyDown(KeyCode.E))
    {
      StartPouring();
      PourMilk();
    }
    else if (Input.GetKeyUp(KeyCode.E))
    {
      StopPouring();
    }

    // Update rotation
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
  }

  private void CheckForContainers()
  {
    Collider2D[] overlaps = new Collider2D[10]; // Adjust size as needed
    ContactFilter2D contactFilter = new ContactFilter2D();
    contactFilter.useTriggers = true;
    int count = containerCollider.Overlap(contactFilter, overlaps);

    canPour = count > 0;
    Debug.Log(count);

    for (int i = 0; i < count; i++)
    {
      Tool tool = overlaps[i].gameObject.GetComponent<Tool>();
      Debug.Log("Tool: " + tool);
      if (tool != null && tool.isContainer)
      {
        Debug.Log(tool.gameObject.name);
        canPour = true;
        nearbyContainer = tool;
        return;
      }
    }

    nearbyContainer = null;
  }

  private void StartPouring()
  {
    if (nearbyContainer != null)
    {
      isPouring = true;
      // Set target rotation for pouring
      targetRotation = Quaternion.Euler(pouringRotation) * startRotation;
    }
  }

  private void StopPouring()
  {
    isPouring = false;
    // Return to original rotation
    targetRotation = startRotation;
  }

  private void PourMilk()
  {
    if (nearbyContainer != null && nearbyContainer.isContainer)
    {
      // Pour milk into the container
      IngredientDetails ingredientDetails = InventoryManager.Instance.GetIngredientDetails(ItemId);
      nearbyContainer.InstantiateLiquid(ingredientDetails.liquidPrefab);
    }
  }

  // Optional: Visualize the pour range in the editor
  private void OnDrawGizmosSelected()
  {
    Gizmos.color = Color.yellow;
    Gizmos.DrawWireSphere(transform.position, pourDistance);
  }
}