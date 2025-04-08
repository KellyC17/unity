using System.Collections;
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
  private Quaternion startRotation; // Initial rotation
  private Quaternion targetRotation; // Current target rotation

  private bool isPouring = false;
  private bool canPour = false;
  private Tool nearbyContainer;

  [Header("Trigger Pour related")]
  public Collider2D containerCollider;

  private bool hasPouredLiquid = false;  // Add this field at class level

  private void Start()
  {
    startRotation = transform.rotation;
    targetRotation = startRotation;
  }

  private void Update()
  {
    // Check for nearby containers only if we're not currently pouring
    if (!isPouring)
    {
      CheckForContainers();

      // Handle pouring input
      if (canPour)
      {
        StartPouring();
      }
    }

    // Update rotation
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
  }

  private void CheckForContainers()
  {
    Collider2D[] overlaps = new Collider2D[10];
    ContactFilter2D contactFilter = new ContactFilter2D();
    contactFilter.useTriggers = true;
    int count = containerCollider.Overlap(contactFilter, overlaps);

    canPour = false; // Reset canPour
    nearbyContainer = null; // Reset nearbyContainer

    for (int i = 0; i < count; i++)
    {
      Tool tool = overlaps[i].gameObject.GetComponent<Tool>();
      if (tool != null && tool.isContainer)
      {
        canPour = true;
        nearbyContainer = tool;
        return;
      }
    }
  }

  private void StartPouring()
  {
    if (nearbyContainer != null && !isPouring)
    {
      isPouring = true;
      canPour = false; // Prevent additional pour attempts
      
      // Calculate direction to nearby container
      Vector2 directionToContainer = (nearbyContainer.transform.position - transform.position).normalized;
      float angle = Mathf.Atan2(directionToContainer.y, directionToContainer.x) * Mathf.Rad2Deg;
      
      // Rotate 90 degrees more than the direction to container
      Quaternion targetDir = Quaternion.Euler(0, 0, angle - 90);
      targetRotation = targetDir;
      
      StartCoroutine(PourRoutine());
    }
  }

  private IEnumerator PourRoutine()
  {
    // Wait for 1 second while pouring
    yield return new WaitForSeconds(0.1f);
    
    PourMilk();
    StopPouring();
  }

  private void StopPouring()
  {
    isPouring = false;
    targetRotation = startRotation;
  }

  private void PourMilk()
  {
    if (nearbyContainer != null && nearbyContainer.isContainer && !hasPouredLiquid)
    {
      // Pour milk into the container
      IngredientDetails ingredientDetails = InventoryManager.Instance.GetIngredientDetails(ItemId);
      nearbyContainer.InstantiateLiquid(ingredientDetails.liquidPrefab);
      hasPouredLiquid = true;  // Mark that we've poured the liquid
    }
  }
}