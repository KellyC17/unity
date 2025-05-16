using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientDetails : ItemDetails
{
  public bool canBeChopped;
  public string choppedTransformItemId;
  public GameObject choppedPrefab;
  public GameObject liquidPrefab;
  public bool isLiquid;
  public bool canBeEatten;

  // Container-specific snap positions
  public Vector3 bowlSnapPosition = new Vector3(0, 0.5f, 0);
  public Vector3 blenderSnapPosition = new Vector3(-0.1f, 0f, 0f);
  public Vector3 steamerSnapPosition = new Vector3(0f, 0f, 0f);

  public string[] foodProducable;

  public int minFoodProduceAmount;
  public int maxFoodProduceAmount;
}