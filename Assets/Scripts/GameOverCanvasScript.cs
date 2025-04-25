using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverCanvasScript : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public Button playAgainButton;
    public Button exitButton;
    private GameObject content;

    void Start()
    {
        content = transform.Find("Content").gameObject;
        Transform layout = transform.Find("Content");
        playAgainButton.onClick.AddListener(OnPlayAgainClicked);
        exitButton.onClick.AddListener(OnExitClicked);

    }

    public void SetTitle(string text)
    {
        if (titleText != null)
            titleText.text = text;
    }
    public void Show(string title)
    {
        SetTitle(title);
        Time.timeScale = 0f;
        content.SetActive(true);
    }

    private void OnPlayAgainClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnExitClicked()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
