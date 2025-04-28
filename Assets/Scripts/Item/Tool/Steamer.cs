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
    // Start coroutine to increment progress over 60 seconds
    StartCoroutine(UpdateProgress(progressBar));
    // }
  }

  public override void InstantiateLiquid(GameObject prefab)
  {
    // this position is specific to the blender image
    var position = transform.position - new Vector3(0.1f, 0f, 0f);
    Instantiate(prefab, position, Quaternion.identity);
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
    float duration = 10f; 
    float elapsedTime = 0f;

    while (elapsedTime < duration)
    {
        elapsedTime += Time.deltaTime;  // Increase the elapsed time
        progressBar.fillImage.fillAmount = Mathf.Lerp(0f, 1f, elapsedTime / duration);
        yield return null;
    }

    // When progress is complete, set isReady to true
    isSteaming = false;
    Debug.Log("Steaming process completed!");
  }
}
