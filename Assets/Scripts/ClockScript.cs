using UnityEngine;
using TMPro;

public class ClockScript : MonoBehaviour
{
    public DayNightManager dayNightManager; // Ссылка на менеджер дня и ночи
    public TextMeshProUGUI clockText; // Ссылка на TextMeshPro для отображения времени

    private int hours;
    private int minutes;

    private const int START_HOUR = 7; // Начальное время (7:00 утра)

    void Update()
    {
        if (dayNightManager == null || clockText == null) return; // Проверка на null

        // Перевод TimeOfDay (0.0 - 1.0) в 24-часовой формат
        float totalMinutes = dayNightManager.TimeOfDay * 24 * 60;
        hours = (Mathf.FloorToInt(totalMinutes / 60) + START_HOUR) % 24; // Часы (0-23)
        minutes = Mathf.FloorToInt(totalMinutes % 60); // Минуты (0-59)

        // Форматируем строку: например, "13:05"
        clockText.text = $"{hours:00}:{minutes:00}";
    }
}
