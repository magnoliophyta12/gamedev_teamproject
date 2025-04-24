using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class MenuDifficultyScript : MonoBehaviour
{
    [SerializeField]
    private CharacterScript playerCharacter;
    [SerializeField]
    private HealthBarScript healthBarScript;

    private Slider speedSlider;

    private Slider incSlider;
    private Slider decSlider;

    private const string SpeedKey = "PlayerSpeed";
    private const string IncKey = "Health_IncAmount";
    private const string DecKey = "Health_DecAmount";

    void Start()
    {

    }
    void OnEnable()
    {
        MenuExitScript.SceneReady += OnSceneReady;
    }

    void OnDisable()
    {
        MenuExitScript.SceneReady -= OnSceneReady;
    }

    private void OnSceneReady(CharacterScript player, HealthBarScript healthBar)
    {

        playerCharacter = player;
        healthBarScript = healthBar;

        InitializeSliders();
    }
    private void InitializeSliders()
    {
        Transform layout = transform.Find("Content/Difficulty/Layout");
        speedSlider = layout.Find("PlayerSpeed/Slider").GetComponent<Slider>();
        incSlider = layout.Find("HealthRecovery/Slider").GetComponent<Slider>();
        decSlider = layout.Find("HealthDrain/Slider").GetComponent<Slider>();

        LoadValue(speedSlider, SpeedKey, value => playerCharacter.SetSpeed(value));
        LoadValue(incSlider, IncKey, value => healthBarScript.SetIncAmount(value));
        LoadValue(decSlider, DecKey, value => healthBarScript.SetDecAmount(value));

        speedSlider.onValueChanged.AddListener(OnSpeedSliderChanged);
        incSlider.onValueChanged.AddListener(OnIncSliderChanged);
        decSlider.onValueChanged.AddListener(OnDecSliderChanged);
    }

    public void OnSpeedSliderChanged(float value)
    {
        SetSpeed(value);
        PlayerPrefs.SetFloat(SpeedKey, value);
    }

    private void SetSpeed(float speed)
    {
        if (playerCharacter != null)
        {
            playerCharacter.SetSpeed(speed);
        }
    }
    public void OnIncSliderChanged(float value)
    {
        healthBarScript.SetIncAmount(value);
        PlayerPrefs.SetFloat(IncKey, value);
    }

    public void OnDecSliderChanged(float value)
    {
        healthBarScript.SetDecAmount(value);
        PlayerPrefs.SetFloat(DecKey, value);
    }
     private void LoadValue(Slider slider, string key, System.Action<float> setter)
    {
        float value = PlayerPrefs.GetFloat(key, 0.3f);
        slider.value = value;
        setter(value);
    }
}