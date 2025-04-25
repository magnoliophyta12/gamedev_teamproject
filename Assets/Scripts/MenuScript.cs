using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    private static MenuScript prevInstance = null;
    private GameObject content;
    void Start()
    {
        if (prevInstance == null)
        {
            prevInstance = this;
            content = transform.Find("Content").gameObject;
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        Time.timeScale = gameObject.activeInHierarchy ? 0.0f : 1.0f;

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            Time.timeScale = 1 - Time.timeScale;
            content.SetActive(!content.activeInHierarchy);
            MenuKeybindingsScript.SetMenuState(content.activeInHierarchy);
        }
        if (Input.GetKeyUp(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(1);
        }
        if (Input.GetKeyUp(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(2);
        }
    }
}
