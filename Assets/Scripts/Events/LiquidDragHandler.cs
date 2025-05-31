using UnityEngine;
using UnityEngine.EventSystems;

public class LiquidDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public LiquidContainer sourceContainer;

    public Steamer steamer;   // nullable: assigned when spawned in Steamer
    public Blender blender;   // nullable: assigned when spawned in Blender

    private Vector3 originalPosition;
    private Transform originalParent;

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;
        originalParent = transform.parent;

        if (steamer != null && sourceContainer != null)
        {
            steamer.OnLiquidDraggedOut(sourceContainer);
        }
        else if (blender != null && sourceContainer != null)
        {
            blender.OnLiquidDraggedOut(sourceContainer);
        }

        transform.SetParent(null);
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0;
        transform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Snap back logic or handle drop into other containers
        transform.position = originalPosition;
        transform.SetParent(originalParent);
    }
}
