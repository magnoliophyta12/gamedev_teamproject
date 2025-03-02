using UnityEngine;
using UnityEngine.UI;

public class MapScaler : MonoBehaviour
{
    public RectTransform mapRect; // Ссылка на UI-объект карты
    public float sizeRatio = 0.9f;  // Процент экрана (например, 30%)

    void Start()
    {
        ResizeMap();
    }

    void Update()
    {
        ResizeMap(); // Проверяем размер карты в реальном времени
    }

    void ResizeMap()
    {
        if (mapRect == null) return;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Берём минимальный размер экрана и умножаем на `sizeRatio`
        float newSize = Mathf.Min(screenWidth, screenHeight) * sizeRatio;

        // Устанавливаем одинаковую ширину и высоту (квадрат)
        mapRect.sizeDelta = new Vector2(newSize, newSize);
    }
}
