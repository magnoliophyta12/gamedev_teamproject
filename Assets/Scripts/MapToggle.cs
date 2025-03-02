using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapUI; // UI с картой

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) // Если нажата M
        {
            mapUI.SetActive(!mapUI.activeSelf); // Переключаем отображение
        }
    }
}
