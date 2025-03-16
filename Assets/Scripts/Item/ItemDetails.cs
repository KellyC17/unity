using System.ComponentModel;
using UnityEngine;

[System.Serializable]
public abstract class ItemDetails
{
  [ItemIdDescription]
  public string itemId;
  public string name;
  public string description;
  public Sprite sprite;
  public bool canBeMoved;
  public bool canBeDragged;
}