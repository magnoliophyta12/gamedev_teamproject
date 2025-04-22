using UnityEngine;
using UnityEngine.TextCore.Text;

public class CompassScript : MonoBehaviour
{
    public RectTransform arrow; // Стрелка на компасе
    public Transform player; // Игрок
    private Transform home; // Дом (цель)
    
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

        Vector3 d = home.position - player.position;
        Vector3 f = Camera.main.transform.forward;
        d.y = 0f;
        f.y = 0f;
        float compasAngle = Vector3.SignedAngle(f, d, Vector3.down);
        arrow.eulerAngles = new Vector3(0, 0, compasAngle);
    }
}
