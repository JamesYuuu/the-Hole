using UnityEngine;
using UnityEngine.UI;

public class UIScaleFromBottom : MonoBehaviour
{
    public GameObject parentObject;
    public float scaleValue;

    private RectTransform rectTransform;
    private float initialHeight;

    void Start()
    {
        rectTransform = parentObject.GetComponent<RectTransform>();
        initialHeight = rectTransform.rect.height;
    }

    void Update()
    {
        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;
        float height = initialHeight * scaleValue;

        anchorMin.y = 0f;
        anchorMax.y = scaleValue;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, height);
    }
}
