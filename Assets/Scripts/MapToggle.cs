using UnityEngine;

public class MapToggle : MonoBehaviour
{
    public GameObject mapUI; 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M)) 
        {
            mapUI.SetActive(!mapUI.activeSelf); 
        }
    }
}
