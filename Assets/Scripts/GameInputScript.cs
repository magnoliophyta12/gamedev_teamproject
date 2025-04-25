using UnityEngine;

public class GameInputScript : MonoBehaviour
{
    private KeyCode runKey;
    private KeyCode pickUpKey;
    private KeyCode attackKey;
    private KeyCode jumpKey;

    void Start()
    {
        runKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Run", "LeftShift"));
        pickUpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_PickUp", "E"));
        attackKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Attack", "Mouse0"));
        jumpKey = (KeyCode)System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Key_Jump", "Space"));
    }

    void Update()
    {
        if (Input.GetKeyDown(runKey)) Debug.Log("Run");
        if (Input.GetKeyDown(pickUpKey)) Debug.Log("Pick Up");
        if (Input.GetKeyDown(attackKey)) Debug.Log("Attack");
        if (Input.GetKeyDown(jumpKey)) Debug.Log("Jump");
    }
}