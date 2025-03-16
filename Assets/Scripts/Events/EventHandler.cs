
using System.Collections.Generic;
using UnityEngine;
public delegate void MovementDelegate(ToolEffect toolEffect, List<Ingredient> ingredients);

public static class EventHandler
{

  public static event MovementDelegate OnMovement;

  public static void Movement(ToolEffect toolEffect, List<Ingredient> ingredients)
  {
    if (OnMovement != null)
    {
      OnMovement(toolEffect, ingredients);
    }
  }
}