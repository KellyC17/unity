using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class IngredientDetails : ItemDetails
{
  public bool canBeChopped;
  public string choppedTransformItemId;
  public GameObject choppedPrefab;
  public GameObject liquidPrefab;
  public bool isSteamable;
  public string steamedTransformItemId;

  public bool canBeBlended; // 搅拌机
  public string pureedTransformItemId;

  public bool canBeMixed; // 厨师机
  public string mixedTransformItemId;

  public bool canBeEatten;

  public string[] foodProducable;

  public int minFoodProduceAmount;
  public int maxFoodProduceAmount;

}