using UnityEngine;

public class MapScaler : MonoBehaviour
{
    public RectTransform mapRect; 
    public float sizeRatio = 0.3f; 

    void Start()
    {
        ResizeMap();
    }

    void Update()
    {
        ResizeMap(); 
    }

    void ResizeMap()
    {
        if (mapRect == null) return;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        float newSize = Mathf.Min(screenWidth, screenHeight) * sizeRatio;

        mapRect.sizeDelta = new Vector2(newSize, newSize);
    }
}
