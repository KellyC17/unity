using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image fillImage;
    public float maxValue = 100f;
    public float currentValue = 0f;

    public void Update()
    {
        fillImage.fillAmount = currentValue / maxValue;
    }
}