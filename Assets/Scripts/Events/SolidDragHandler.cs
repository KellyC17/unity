using UnityEngine;
using UnityEngine.EventSystems;

public class SolidDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Steamer steamer;   // Nullable: assigned when spawned in Steamer
    public Blender blender;   // Nullable: assigned when spawned in Blender
    public Mixer mixer;       // nullable: assigned when spawned in Mixer

    public Vector3 originalPosition; // Optional: track original position if needed

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = transform.position;

        if (steamer != null)
        {
            steamer.OnSolidDraggedOut(gameObject);
        }
        else if (blender != null)
        {
            blender.OnSolidDraggedOut(gameObject);
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 worldPoint = Camera.main.ScreenToWorldPoint(eventData.position);
        worldPoint.z = 0; // Ensure the object stays in the correct plane
        transform.position = worldPoint;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // No snap-back logic; solid objects can remain wherever they are dropped
        Debug.Log("Solid object dropped at: " + transform.position);
    }
}