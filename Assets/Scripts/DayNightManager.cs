using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [Range(0, 1)]
    public float TimeOfDay; 

    public float DayDuration = 450f; 
    public float NightDuration = 200f; 

    private float cycleDuration;
    private bool isReversing = false; 
    private float isReversingCooldown = 1f; 

    public Material DaySkybox;
    public Material NightSkybox;

    public Light Sun;  
    public Light Moon; 

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceAmbient;
    public AudioClip DayMusic;
    public AudioClip NightMusic;
    public AudioClip DayAmbient;
    public AudioClip NightAmbient;

    private float sunIntensity;
    private float moonIntensity;
    private bool isDay = true; 

    public GoalSpawner goalSpawner;

    public GameObject lostText;
    private bool gameOverTriggered = false;
    private void Start()
    {
        sunIntensity = Sun.intensity;
        moonIntensity = Moon.intensity;
        cycleDuration = DayDuration + NightDuration;

        PlayMusic(DayMusic, true);
        PlayAmbient(DayAmbient, true);

        if (isDay)
        {
            Debug.Log("[DayNightManager] First day start. Spawning chest immediately.");
            goalSpawner.SpawnChest();
        }
    }

    private void Update()
    {
        if (TimeOfDay >= 1 && !isReversing)
        {
            isReversing = true;
            isReversingCooldown = 1f; 
        }

        if (isReversing)
        {
            isReversingCooldown -= Time.deltaTime;
            if (isReversingCooldown <= 0)
            {
                TimeOfDay -= Time.deltaTime / (cycleDuration * 2f); 

                if (TimeOfDay <= 0.8f)
                {
                    TimeOfDay = 0f;
                    isReversing = false;
                }
            }
        }
        else
        {
            TimeOfDay += Time.deltaTime / cycleDuration;
        }

        float dayEnd = DayDuration / cycleDuration; 

        RenderSettings.skybox = TimeOfDay > dayEnd ? NightSkybox : DaySkybox;
        DynamicGI.UpdateEnvironment();

        float sunAngle = Mathf.Lerp(-90f, 270f, TimeOfDay);
        float moonAngle = sunAngle + 180f;

        Sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        Moon.transform.rotation = Quaternion.Euler(moonAngle, -170f, 0f);

        float sunriseFactor = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(dayEnd - 0.1f, dayEnd, TimeOfDay));
        float sunsetFactor = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(dayEnd, dayEnd + 0.1f, TimeOfDay));

        Sun.intensity = sunIntensity * (Mathf.SmoothStep(0, 1, Mathf.Sin(TimeOfDay * Mathf.PI)) * 0.9f + sunriseFactor * 0.2f);
        Moon.intensity = moonIntensity * (Mathf.SmoothStep(0, 1, Mathf.Sin((TimeOfDay + 0.5f) * Mathf.PI)) * 0.9f + sunsetFactor * 0.2f);

        if (TimeOfDay > dayEnd && isDay) 
        {
            isDay = false;
            PlayAmbient(NightAmbient, true);
            PlayMusic(NightMusic, true); 

            goalSpawner.ResetChestSpawn();
        }
        else if (TimeOfDay <= dayEnd && !isDay) 
        {
            isDay = true;
            PlayAmbient(DayAmbient, true);
            PlayMusic(DayMusic, true); 

            goalSpawner.SpawnChest();
        }

        if (!gameOverTriggered && TimeOfDay >= 0.97f && isDay)
        {
            gameOverTriggered = true;
            TriggerGameOver();
            return; 
        }
    }

    public void PlayMusic(AudioClip clip, bool loop)
    {
        if (audioSourceMusic.isPlaying)
            audioSourceMusic.Stop();

        audioSourceMusic.clip = clip;
        audioSourceMusic.loop = loop;
        audioSourceMusic.Play();
    }
    public void PlayAmbient(AudioClip clip, bool loop)
    {
        if (audioSourceAmbient.isPlaying)
            audioSourceAmbient.Stop();

        audioSourceAmbient.clip = clip;
        audioSourceAmbient.loop = loop;
        audioSourceAmbient.Play();
    }

    public void TriggerGameOver()
    {
        Debug.Log("[DayNightManager] Game Over!");

        audioSourceMusic.Stop();
        audioSourceAmbient.Stop();

        Time.timeScale = 0f;

        if (lostText != null)
        {
            lostText.SetActive(true);
        }
    }
}