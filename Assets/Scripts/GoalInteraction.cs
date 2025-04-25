using UnityEngine;
using TMPro;

public class GoalInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public KeyCode interactKey = KeyCode.E;
    public DayNightManager dayNightManager;
    public TextMeshProUGUI hintText;

    private GameObject currentGoal;

    private void Update()
    {
        currentGoal = FindClosestGoal();

        if (hintText != null)
        {
            hintText.gameObject.SetActive(currentGoal != null);
            if (currentGoal != null)
                hintText.text = "Press "+ interactKey.ToString();
        }

        if (Input.GetKeyDown(interactKey) && currentGoal != null)
        {
            Debug.Log("[GoalInteraction] Взаимодействие с целью. Новый день - новая цель!");

            if (dayNightManager != null && dayNightManager.goalSpawner != null)
            {
                dayNightManager.goalSpawner.ResetChestSpawn();
                dayNightManager.goalSpawner.SpawnChest();

                dayNightManager.TimeOfDay = 0f;

                var dayField = typeof(DayNightManager).GetField("isDay", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (dayField != null)
                    dayField.SetValue(dayNightManager, true);

                dayNightManager.PlayAmbient(dayNightManager.DayAmbient, true);
                dayNightManager.PlayMusic(dayNightManager.DayMusic, true);
            }
        }
    }

    GameObject FindClosestGoal()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        foreach (GameObject goal in goals)
        {
            if (Vector3.Distance(transform.position, goal.transform.position) <= interactionDistance)
                return goal;
        }
        return null;
    }
}