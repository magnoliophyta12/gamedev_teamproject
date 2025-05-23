using UnityEngine;
using TMPro;
using static Unity.Collections.Unicode;

public class GoalInteraction : MonoBehaviour
{
    public float interactionDistance = 3f;
    public DayNightManager dayNightManager;
    public TextMeshProUGUI hintText;

    private KeyCode interactKey;

    private GameObject currentGoal;
    void Start()
    {
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_PickUp", "E"));
    }
    private void Update()
    {
        currentGoal = FindClosestGoal();

        if (hintText != null)
        {
            hintText.gameObject.SetActive(currentGoal != null);
            if (currentGoal != null)
                hintText.text = "Press "+ interactKey.ToString();
        }

        if (Input.GetKeyDown(interactKey) && currentGoal != null && !MenuKeybindingsScript.IsMenuOpen)
        {
            GameOverCanvasScript gameOverUI = FindAnyObjectByType<GameOverCanvasScript>();
            if (gameOverUI != null)
            {
                gameOverUI.Show("You Won!");
            }
            /*
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
            }*/
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
    public void ReloadKeys()
    {
        interactKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_PickUp", "E"));
    }
}