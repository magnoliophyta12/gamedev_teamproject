using UnityEngine;

public class CompassScript : MonoBehaviour
{
    public Transform player; // Игрок
    private Transform home; // Дом (цель)
    public RectTransform arrow; // Стрелка на компасе

    void Start()
    {
        // Находим объект с тегом "Home"
        GameObject homeObject = GameObject.FindGameObjectWithTag("Home");
        if (homeObject != null)
        {
            home = homeObject.transform;
        }
        else
        {
            Debug.LogError("Дом (Home) не найден! Убедитесь, что объект имеет тег 'Home'");
        }
    }

    void Update()
    {
        if (home == null || player == null || arrow == null) return;

        // Вычисляем направление к дому (без учета высоты)
        Vector3 directionToHome = home.position - player.position;
        directionToHome.y = 0; // Игнорируем разницу по высоте

        // Определяем угол между направлением игрока и направлением к дому
        float angle = Vector3.SignedAngle(player.forward, directionToHome, Vector3.up);

        // Устанавливаем поворот стрелки в UI
        arrow.localRotation = Quaternion.Euler(0, 0, -angle);
    }
}
