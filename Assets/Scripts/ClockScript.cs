using UnityEngine;
using TMPro;

public class ClockScript : MonoBehaviour
{
    public DayNightManager dayNightManager; 
    public TextMeshProUGUI clockText; 

    private int hours;
    private int minutes;

    private const int START_HOUR = 7; 

    void Update()
    {
        float totalMinutes = dayNightManager.TimeOfDay * 24 * 60;
        hours = (Mathf.FloorToInt(totalMinutes / 60) + START_HOUR) % 24; 
        minutes = Mathf.FloorToInt(totalMinutes % 60); 

        clockText.text = $"{hours:00}:{minutes:00}";
    }
}
