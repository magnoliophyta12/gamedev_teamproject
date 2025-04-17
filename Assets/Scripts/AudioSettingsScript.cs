using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsScript : MonoBehaviour
{
    [SerializeField]
    private AudioMixer audioMixer;
    private const string ambientParam = "AmbientVolume";
    private const string effectsParam = "EffectsVolume";
    private const string musicParam = "MusicVolume";
    private const string masterParam = "MasterVolume";

    void Start()
    {
       /* audioMixer.SetFloat(ambientParam, -10.0f);
        audioMixer.SetFloat(effectsParam, -8.0f);
        audioMixer.SetFloat(musicParam, -4.0f);*/
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.KeypadPlus) || Input.GetKeyDown(KeyCode.Plus))
        {
            ChangeMasterVolume(isLouder: true);
        }
        if (Input.GetKeyDown(KeyCode.KeypadMinus) || Input.GetKeyDown(KeyCode.Minus))
        {
            ChangeMasterVolume(isLouder: false);
        }
    }
    private void ChangeMasterVolume(bool isLouder)
    {
        float masterVolume;
        if (audioMixer.GetFloat(masterParam, out masterVolume))
        {
            float step = 5 + Mathf.Abs(masterVolume + 5) * 0.25f;
            masterVolume = Mathf.Clamp(
                isLouder ? masterVolume + step : masterVolume - step, -80, 20);
            audioMixer.SetFloat(nameof(masterParam), masterVolume);
        }
    }
}
