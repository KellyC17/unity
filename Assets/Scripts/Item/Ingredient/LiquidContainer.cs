using UnityEngine;

public class LiquidContainer : Ingredient
{
  [Header("Pouring Settings")]
  [SerializeField] private float pourDistance = 2f;
  [SerializeField] private Transform pourPoint;

  [Header("Rotation Settings")]
  [SerializeField] private float rotationSpeed = 5f;
  private Quaternion startRotation;
  private Quaternion targetRotation;

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
    if (!isPouring)
    {
      CheckForContainers();
      if (canPour)
      {
        StartPouring();
      }
    }
    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
  }

  private void CheckForContainers()
  {
    Collider2D[] overlaps = new Collider2D[10];
    ContactFilter2D contactFilter = new ContactFilter2D();
    contactFilter.useTriggers = true;
    int count = containerCollider.Overlap(contactFilter, overlaps);

    canPour = false;
    nearbyContainer = null;

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
      canPour = false;

      Vector2 directionToContainer = (nearbyContainer.transform.position - transform.position).normalized;
      float angle = Mathf.Atan2(directionToContainer.y, directionToContainer.x) * Mathf.Rad2Deg;
      Quaternion targetDir = Quaternion.Euler(0, 0, angle - 90);
      targetRotation = targetDir;

      PourLiquid();
    }
  }

  private void PourLiquid()
  {
    if (nearbyContainer != null && nearbyContainer.isContainer)
    {
      IngredientDetails ingredientDetails = InventoryManager.Instance.GetIngredientDetails(ItemId);
      nearbyContainer.ReceiveLiquid(ingredientDetails.liquidPrefab, this);
      Invoke("StopPouring", 0.5f);
    }
  }

  private void StopPouring()
  {
    isPouring = false;
    targetRotation = startRotation;
  }
}
