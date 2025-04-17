using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuSoundScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;

    private Slider masterSlider;
    private Slider effectsSlider;
    private Slider ambientSlider;
    private Slider musicSlider;

    private const string MasterKey = "Volume_Master";
    private const string EffectsKey = "Volume_Effects";
    private const string AmbientKey = "Volume_Ambient";
    private const string MusicKey = "Volume_Music";

    void Start()
    {
        Transform layout = transform.Find("Content/Sound/Layout");

        masterSlider = layout.Find("Master/Slider").GetComponent<Slider>();
        effectsSlider = layout.Find("Effects/Slider").GetComponent<Slider>();
        ambientSlider = layout.Find("Ambient/Slider").GetComponent<Slider>();
        musicSlider = layout.Find("Music/Slider").GetComponent<Slider>();

        LoadVolume(masterSlider, MasterKey, "MasterVolume");
        LoadVolume(effectsSlider, EffectsKey, "EffectsVolume");
        LoadVolume(ambientSlider, AmbientKey, "AmbientVolume");
        LoadVolume(musicSlider, MusicKey, "MusicVolume");

        masterSlider.onValueChanged.AddListener(OnMasterSliderChanged);
        effectsSlider.onValueChanged.AddListener(OnEffectsSliderChanged);
        ambientSlider.onValueChanged.AddListener(OnAmbientSliderChanged);
        musicSlider.onValueChanged.AddListener(OnMusicSliderChanged);
    }

    private void LoadVolume(Slider slider, string key, string mixerParam)
    {
        float value = PlayerPrefs.GetFloat(key, 1f);
        slider.value = value;
        audioMixer.SetFloat(mixerParam, ValueToDb(value));
    }

    public void OnMasterSliderChanged(float value)
    {
        audioMixer.SetFloat("MasterVolume", ValueToDb(value));
        PlayerPrefs.SetFloat(MasterKey, value);
    }

    public void OnEffectsSliderChanged(float value)
    {
        audioMixer.SetFloat("EffectsVolume", ValueToDb(value));
        PlayerPrefs.SetFloat(EffectsKey, value);
    }

    public void OnAmbientSliderChanged(float value)
    {
        audioMixer.SetFloat("AmbientVolume", ValueToDb(value));
        PlayerPrefs.SetFloat(AmbientKey, value);
    }

    public void OnMusicSliderChanged(float value)
    {
        audioMixer.SetFloat("MusicVolume", ValueToDb(value));
        PlayerPrefs.SetFloat(MusicKey, value);
    }

    private float ValueToDb(float value)
    {
        return -80.0f + 100f * Mathf.Sqrt(value);
    }
    private float DbToValue(float db)
    {
        return Mathf.Pow((db + 80f) / 100f, 2f);
    }
}