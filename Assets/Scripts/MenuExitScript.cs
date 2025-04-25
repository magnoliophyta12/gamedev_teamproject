using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuExitScript : MonoBehaviour
{
    public delegate void OnSceneReady(CharacterScript player, HealthBarScript healthBar);
    public static event OnSceneReady SceneReady;

    private CharacterScript playerCharacter { get; set; }
    private HealthBarScript healthBar { get; set; }

    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("MenuCanvas"));
        DontDestroyOnLoad(GameObject.Find("MenuManager"));
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(1);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {

            playerCharacter = GameObject.FindGameObjectWithTag("Player")?.GetComponent<CharacterScript>();

            healthBar = GameObject.FindGameObjectWithTag("HealthBar")?.GetComponent<HealthBarScript>();

            if (playerCharacter != null && healthBar != null)
            {
                SceneReady?.Invoke(playerCharacter, healthBar);
            }
        }
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}