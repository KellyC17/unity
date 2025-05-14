using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Steamer : Tool
{
  [SerializeField]
  public BoxCollider2D SteamTriggerCollider;
  public override bool isContainer => true;
  private bool isSteaming = false;
  public ProgressBar progressBar;

  public override void Update()
  {
    if (Input.GetMouseButtonDown(0) && IsMouseOver())  // Left click and mouse is over the object
    {
      if (!isSteaming)
      {
        ApplyToolAction();
      }
    }
  }

  public override void ApplyToolAction()
  {
    // Only start steaming if we have any liquids
    if (activeLiquids.Count == 0) return;

    progressBar.maxValue = 60;
    progressBar.currentValue = 0;
    StartCoroutine(UpdateProgress(progressBar));
  }

  // New method to handle receiving liquid
  public override void ReceiveLiquid(GameObject liquidPrefab, LiquidContainer sourceContainer)
  {
    // Don't accept liquid from the same source
    if (activeLiquids.ContainsKey(sourceContainer)) return;

    // Instantiate new liquid
    var position = transform.position - new Vector3(0.1f, 0f, 0f);
    GameObject newLiquid = Instantiate(liquidPrefab, position, Quaternion.identity);
    activeLiquids.Add(sourceContainer, newLiquid);
  }

  private bool IsMouseOver()
  {
    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    return SteamTriggerCollider.OverlapPoint(mousePosition);
  }

  private IEnumerator UpdateProgress(ProgressBar progressBar)
  {
    Debug.Log("Updating progress");
    isSteaming = true;
    float duration = 30f;
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
      elapsedTime += Time.deltaTime;
      progressBar.currentValue = Mathf.Lerp(0f, progressBar.maxValue, elapsedTime / duration);
      yield return null;
    }

    // When progress is complete
    isSteaming = false;

    Debug.Log("Steaming process completed!");
  }
}
