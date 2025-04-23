using UnityEngine;
using UnityEngine.UI;

public class GoalMarkerUI : MonoBehaviour
{
    public Camera mapCamera;       // Камера, которая рендерит карту
    public RectTransform markerUI; // UI-элемент (Image)
    private Transform goalTarget;

    private float searchCooldown = 1f;
    private float searchTimer = 0f;

    void Update()
    {
        if (goalTarget == null)
        {
            searchTimer -= Time.deltaTime;
            if (searchTimer <= 0f)
            {
                GameObject goalObj = GameObject.FindWithTag("Goal");
                if (goalObj != null)
                {
                    goalTarget = goalObj.transform;
                    Debug.Log("[GoalMarkerUI] Найден новый Goal: " + goalTarget.name);
                }
                searchTimer = searchCooldown;
            }
            markerUI.gameObject.SetActive(false);
            return;
        }

        if (mapCamera == null || markerUI == null)
            return;

        Vector3 screenPos = mapCamera.WorldToScreenPoint(goalTarget.position);
        if (screenPos.z > 0)
        {
            markerUI.position = screenPos;
            markerUI.gameObject.SetActive(true);
        }
        else
        {
            markerUI.gameObject.SetActive(false);
        }
    }
}