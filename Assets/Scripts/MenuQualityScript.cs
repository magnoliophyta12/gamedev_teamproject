using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuQualityScript : MonoBehaviour
{
    private TMPro.TMP_Dropdown graphicsDropdown;
    private TMPro.TMP_Dropdown fogDropdown;
    private TMPro.TMP_Dropdown screenModeDropdown;
    private UnityEngine.UI.Slider gammaSlider;

    void Start()
    {
        Transform layout = transform.Find("Content/Quality/Layout");

        graphicsDropdown = layout.Find("Graphics/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        InitGraphicsDropdown();

        fogDropdown = layout.Find("Fog/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        InitFogDropdown();

        screenModeDropdown = layout.Find("ScreenMode/Dropdown").GetComponent<TMPro.TMP_Dropdown>();
        InitScreenModeDropdown();

        gammaSlider = layout.Find("Gamma/Slider").GetComponent<UnityEngine.UI.Slider>();
        gammaSlider.onValueChanged.AddListener(OnGammaChanged);
        gammaSlider.value = PlayerPrefs.GetFloat("Gamma", 1f);
        SetGamma(gammaSlider.value);
        ApplyFogSettings();


        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ApplyFogSettings();
    }

    private void ApplyFogSettings()
    {
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.6588f, 0.7921f, 0.9137f);
        RenderSettings.fogDensity = 0.02f;
        RenderSettings.fogMode = FogMode.Exponential;
    }


    private void InitFogDropdown()
    {
        fogDropdown.ClearOptions();
        fogDropdown.options.Add(new("Off"));
        foreach (string name in System.Enum.GetNames(typeof(FogMode)))
        {
            fogDropdown.options.Add(new(name));
        }

        if (RenderSettings.fog)
        {
            fogDropdown.value = (int)RenderSettings.fogMode;
        }
        else
        {
            fogDropdown.value = 0;
        }
    }

    public void OnFogDropdownChanged(int selectedIndex)
    {
        if (selectedIndex == 0)
        {
            RenderSettings.fog = false;
        }
        else
        {
            RenderSettings.fog = true;
            RenderSettings.fogMode = (FogMode)selectedIndex;
            RenderSettings.fogDensity = 0.02f;
            RenderSettings.fogColor = new Color(0.6588f, 0.7921f, 0.9137f);
        }
    }

    private void InitGraphicsDropdown()
    {
        graphicsDropdown.ClearOptions();
        foreach (string name in QualitySettings.names)
        {
            graphicsDropdown.options.Add(new(name));
        }
        graphicsDropdown.value = QualitySettings.GetQualityLevel();
    }

    public void OnGraphicsDropdownChanged(int selectedIndex)
    {
        QualitySettings.SetQualityLevel(selectedIndex);
    }

    private void InitScreenModeDropdown()
    {
        screenModeDropdown.ClearOptions();
        screenModeDropdown.options.Add(new("Windowed"));
        screenModeDropdown.options.Add(new("Fullscreen"));
        screenModeDropdown.value = Screen.fullScreen ? 1 : 0;
    }

    public void OnScreenModeChanged(int selectedIndex)
    {
        bool fullscreen = selectedIndex == 1;
        Screen.fullScreen = fullscreen;
    }

    public void OnGammaChanged(float value)
    {
        //Camera.main.GetComponent   <GammaEffect>().SetGamma(gammaSlider.value);
        PlayerPrefs.SetFloat("Gamma", value);
    }

    private void SetGamma(float value)
    {
        RenderSettings.ambientLight = Color.white * value;
        Debug.Log(RenderSettings.ambientLight);
    }
}