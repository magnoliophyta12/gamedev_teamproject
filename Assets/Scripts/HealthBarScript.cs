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

    void Update()
    {
        targetHealth -= Time.deltaTime * healthDecreaseRate;           // повільно змінюємо ціль
        targetHealth = Mathf.Clamp(targetHealth, 0f, 1f);

        // Плавне наближення поточного значення до цілі
        currentHealth = Mathf.Lerp(currentHealth, targetHealth, 3f);
        bar.fillAmount = currentHealth;
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
