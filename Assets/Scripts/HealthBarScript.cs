using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    public Image bar;
    private float currentHealth;
    private float targetHealth;

    private float incAmount = 0.3f;
    private float decAmount = 0.3f;

    float healthDecreaseRate = 0.01f;

    void Start()
    {
        currentHealth = 1.0f;
        targetHealth = currentHealth;
        bar.fillAmount = currentHealth;
    }

    void Update()
    {
        targetHealth -= Time.deltaTime * healthDecreaseRate;           // ������� ������� ����
        targetHealth = Mathf.Clamp(targetHealth, 0f, 1f);

        // ������ ���������� ��������� �������� �� ���
        currentHealth = Mathf.Lerp(currentHealth, targetHealth, 3f);
        bar.fillAmount = currentHealth;
    }

    public void IncreaseHealth()
    {
        targetHealth += incAmount;
        //targetHealth = Mathf.Clamp(targetHealth, 0f, 1f);
    }

    public void ReduceHealth()
    {
        currentHealth -= decAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, 1f);
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
