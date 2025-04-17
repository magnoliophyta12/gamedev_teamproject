using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    [Range(0, 1)]
    public float TimeOfDay; // Текущее время суток (0 - утро, 1 - ночь)

    public float DayDuration = 450f;  // 7.5 минут (450 секунд)
    public float NightDuration = 200f; // 3.3 минуты (200 секунд)

    private float cycleDuration;
    private bool isReversing = false; // Флаг реверса времени
    private float isReversingCooldown = 1f; // Небольшая задержка перед откатом

    public Material DaySkybox;
    public Material NightSkybox;

    public Light Sun;  // Солнце
    public Light Moon; // Луна

    public AudioSource audioSourceMusic;
    public AudioSource audioSourceAmbient;
    public AudioClip DayMusic;
    public AudioClip NightMusic;
    public AudioClip DayAmbient;
    public AudioClip NightAmbient;

    private float sunIntensity;
    private float moonIntensity;
    private bool isDay = true; // Текущее состояние дня/ночи

    private void Start()
    {
        sunIntensity = Sun.intensity;
        moonIntensity = Moon.intensity;
        cycleDuration = DayDuration + NightDuration;

        // Устанавливаем начальную музыку (дневную)
        PlayMusic(DayMusic, true);
        PlayAmbient(DayAmbient, true);
    }

    private void Update()
    {
        // Если достигли конца ночи (TimeOfDay = 1), включаем реверс времени
        if (TimeOfDay >= 1 && !isReversing)
        {
            isReversing = true;
            isReversingCooldown = 1f; // Короткая задержка перед откатом
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

        // Меняем Skybox
        RenderSettings.skybox = TimeOfDay > dayEnd ? NightSkybox : DaySkybox;
        DynamicGI.UpdateEnvironment();

        // Вращение солнца и луны (реалистичная дуга)
        float sunAngle = Mathf.Lerp(-90f, 270f, TimeOfDay);
        float moonAngle = sunAngle + 180f;

        Sun.transform.rotation = Quaternion.Euler(sunAngle, 170f, 0f);
        Moon.transform.rotation = Quaternion.Euler(moonAngle, -170f, 0f);

        // Плавные переходы рассвета и заката
        float sunriseFactor = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(dayEnd - 0.1f, dayEnd, TimeOfDay));
        float sunsetFactor = Mathf.SmoothStep(0, 1, Mathf.InverseLerp(dayEnd, dayEnd + 0.1f, TimeOfDay));

        // Улучшенная интенсивность света (мягкие переходы)
        Sun.intensity = sunIntensity * (Mathf.SmoothStep(0, 1, Mathf.Sin(TimeOfDay * Mathf.PI)) * 0.9f + sunriseFactor * 0.2f);
        Moon.intensity = moonIntensity * (Mathf.SmoothStep(0, 1, Mathf.Sin((TimeOfDay + 0.5f) * Mathf.PI)) * 0.9f + sunsetFactor * 0.2f);

        // Проверяем смену дня и ночи
        if (TimeOfDay > dayEnd && isDay) // Ночь началась
        {
            isDay = false;
            PlayAmbient(NightAmbient, true);
            PlayMusic(NightMusic, true); // Запускаем ночную музыку с зацикливанием
        }
        else if (TimeOfDay <= dayEnd && !isDay) // День начался
        {
            isDay = true;
            PlayAmbient(DayAmbient, true);
            PlayMusic(DayMusic, true); // Запускаем дневную музыку без зацикливания
        }
    }

    // Воспроизведение музыки с возможностью зацикливания
    private void PlayMusic(AudioClip clip, bool loop)
    {
        if (audioSourceMusic.isPlaying)
            audioSourceMusic.Stop();

        audioSourceMusic.clip = clip;
        audioSourceMusic.loop = loop;
        audioSourceMusic.Play();
    }
    private void PlayAmbient(AudioClip clip, bool loop)
    {
        if (audioSourceAmbient.isPlaying)
            audioSourceAmbient.Stop();

        audioSourceAmbient.clip = clip;
        audioSourceAmbient.loop = loop;
        audioSourceAmbient.Play();
    }
}