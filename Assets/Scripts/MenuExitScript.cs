using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuExitScript : MonoBehaviour
{

    void Start()
    {
        DontDestroyOnLoad(GameObject.Find("MenuCanvas"));
        SceneManager.LoadScene(1);
    }


    void Update()
    {

    }
}