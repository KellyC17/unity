using UnityEngine;

public class DragRestricted : MonoBehaviour
{
    [SerializeField] private bool isMoveRestrictedToScreen = false;
    [SerializeField] private float dragPriority = 1f; // Higher priority items can be dragged even when overlapping
    [SerializeField] private float dragZoneRadius = 0.5f; // Radius of the area where dragging can be initiated

    private bool dragging = false;
    private Vector3 offset;
    private Vector3 extents;
    private Camera mainCamera;
    private Collider2D itemCollider;
    private static DragRestricted currentlyDragging;
    private Vector3? lastValidPosition = null; // Store the last valid snap position

    private void Start()
    {
        mainCamera = Camera.main;
        itemCollider = GetComponent<Collider2D>();
        extents = GetComponent<SpriteRenderer>().sprite.bounds.extents;
    }

    void Update()
    {
        if (dragging)
        {
            Vector3 pos = mainCamera.ScreenToWorldPoint(Input.mousePosition) + offset;
            if (isMoveRestrictedToScreen)
            {
                Vector3 topRight = mainCamera.ViewportToWorldPoint(Vector3.one);
                Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(Vector3.zero);
                pos.x = Mathf.Clamp(pos.x, bottomLeft.x + extents.x, topRight.x - extents.x);
                pos.y = Mathf.Clamp(pos.y, bottomLeft.y + extents.y, topRight.y - extents.y);
            }
            transform.position = pos;
        }

        // Handle mouse input
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            float distance = Vector2.Distance(mousePosition, transform.position);

            // Check if mouse is within drag zone
            if (distance <= dragZoneRadius)
            {
                // Check if we can start dragging this item
                if (CanStartDragging())
                {
                    StartDragging();
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && dragging)
        {
            StopDragging();
        }
    }

    private bool CanStartDragging()
    {
        // If nothing is being dragged, we can start dragging
        if (currentlyDragging == null)
            return true;

        // If something else is being dragged, check priorities
        if (currentlyDragging.dragPriority < this.dragPriority)
        {
            currentlyDragging.StopDragging();
            return true;
        }

        return false;
    }

    private void StartDragging()
    {
        dragging = true;
        currentlyDragging = this;
        offset = transform.position - mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Temporarily disable collision while dragging
        if (itemCollider != null)
        {
            itemCollider.enabled = false;
        }
    }

    private void StopDragging()
    {
        dragging = false;
        currentlyDragging = null;

        // Re-enable collision after dragging
        if (itemCollider != null)
        {
            itemCollider.enabled = true;
        }

        bool foundValidContainer = false;

        // Check if we're over a container tool
        Collider2D[] colliders = Physics2D.OverlapPointAll(transform.position);
        foreach (Collider2D collider in colliders)
        {
            Tool tool = collider.GetComponent<Tool>();
            if (tool != null && tool.isContainer)
            {
                // Get the ingredient component to access its ID
                Ingredient ingredient = GetComponent<Ingredient>();
                if (ingredient != null)
                {
                    // Look up ingredient details from InventoryManager
                    IngredientDetails details = InventoryManager.Instance.GetIngredientDetails(ingredient.ItemId);
                    Debug.Log(details.isLiquid);
                    if (details != null)
                    {

                        // Get the appropriate snap position based on tool type
                        Vector3 snapPosition;
                        if (tool is Bowl)
                        {
                            snapPosition = details.bowlSnapPosition;
                        }
                        else if (tool is Blender)
                        {
                            snapPosition = details.blenderSnapPosition;
                        }
                        else if (tool is Steamer)
                        {
                            snapPosition = details.steamerSnapPosition;
                        }
                        else
                        {
                            // Default to bowl snap position if tool type is unknown
                            snapPosition = details.bowlSnapPosition;
                        }

                        // Snap to the container's position using the appropriate snap position
                        Vector3 containerPosition = tool.transform.position;
                        Vector3 newPosition = containerPosition + snapPosition;
                        transform.position = newPosition;
                        lastValidPosition = newPosition;
                        foundValidContainer = true;
                    }
                }
                break;
            }
        }

        // If no valid container was found and we have a last valid position, return to it
        if (!foundValidContainer && lastValidPosition.HasValue)
        {
            transform.position = lastValidPosition.Value;
        }
    }

    private void OnDestroy()
    {
        if (currentlyDragging == this)
        {
            currentlyDragging = null;
        }
    }
}