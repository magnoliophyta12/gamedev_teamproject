using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image bar;
    private float currentHealth;
    private float targetHealth;

    private float incAmount = 0.3f;
    private float decAmount = 0.15f;

    float healthDecreaseRate = 0.01f;

    void Start()
    {
        currentHealth = 1.0f;
        targetHealth = currentHealth;
        bar.fillAmount = currentHealth;
    }

    private bool gameOverTriggered = false; // щоб не викликати кілька разів

    void Update()
    {
        targetHealth -= Time.deltaTime * healthDecreaseRate;
        targetHealth = Mathf.Clamp(targetHealth, 0f, 1f);

        currentHealth = Mathf.Lerp(currentHealth, targetHealth, 3f);
        bar.fillAmount = currentHealth;

        if (!gameOverTriggered && currentHealth <= 0.01f)
        {
            gameOverTriggered = true;
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        GameOverCanvasScript gameOverUI = FindAnyObjectByType<GameOverCanvasScript>();
        if (gameOverUI != null)
        {
            gameOverUI.Show("You Lost");
        }
    }

    public void IncreaseHealth()
    {
        targetHealth += incAmount;
    }

    public void ReduceHealth()
    {
        targetHealth -= decAmount;
    }
    public void SetIncAmount(float value)
    {
        incAmount = value;
    }

    public void SetDecAmount(float value)
    {
        healthDecreaseRate = value;
    }
}
