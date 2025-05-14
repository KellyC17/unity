using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Tool : Item
{
  [SerializeField]
  public ToolDetails _toolDetails;
  public InputAction action;
  public bool isReady;
  public Collider2D ingredientCollider;
  public Collider2D[] overlaps = new Collider2D[10];
  public int overlapCount = 0;

  public virtual bool isContainer { get; set; }

  public Dictionary<string, int> ingredients = new Dictionary<string, int>();
  public List<GameObject> collidingObjects = new List<GameObject>();

  // Add liquid tracking dictionary
  protected Dictionary<LiquidContainer, GameObject> activeLiquids = new Dictionary<LiquidContainer, GameObject>();

  public ToolDetails ToolDetails
  {
    get { return _toolDetails; }
    set { _toolDetails = value; }
  }

  private void Awake()
  {
    action.Enable();
  }


  public virtual void Update()
  {
    overlapCount = ingredientCollider.Overlap(new ContactFilter2D(), overlaps);
  }

  void InstantiateInTheMiddle(List<GameObject> objectsList, GameObject newObject)
  {
    if (objectsList.Count == 0)
    {
      Debug.LogWarning("The list is empty, cannot calculate the middle.");
      return;
    }

    // Calculate the average position of all objects in the list
    Vector3 averagePosition = Vector3.zero;
    foreach (GameObject obj in objectsList)
    {
      averagePosition += obj.transform.position;
    }
    averagePosition /= objectsList.Count; // Divide by the number of objects to get the average

    // Instantiate the new object at the average position
    Instantiate(newObject, averagePosition, Quaternion.identity);
  }

  public virtual void ApplyToolAction()
  {
    Debug.Log("Tool action triggered");
  }

  public virtual void InstantiateLiquid(GameObject prefab)
  {
  }

  // Add virtual method for receiving liquid
  public virtual void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
  {
    // Base implementation returns null - only Blender and Steamer should override this
    return;
  }
}
