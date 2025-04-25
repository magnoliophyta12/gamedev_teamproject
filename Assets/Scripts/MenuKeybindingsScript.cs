
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuKeybindingsScript : MonoBehaviour
{
    private Button runButton;
    private Button pickupButton;
    private Button attackButton;
    private Button jumpButton;

    private TMP_Text runText;
    private TMP_Text pickupText;
    private TMP_Text attackText;
    private TMP_Text jumpText;

    private string waitingForKey = null;

    private const string RunKey = "Key_Run";
    private const string PickUpKey = "Key_PickUp";
    private const string AttackKey = "Key_Attack";
    private const string JumpKey = "Key_Jump";
    public static bool IsMenuOpen { get; private set; } = true;

    public static void SetMenuState(bool isOpen)
    {
        IsMenuOpen = isOpen;
    }

    void Start()
    {
        Transform layout = GameObject.Find("MenuCanvas/Content/Controls/Layout").transform;

        runButton = layout.Find("Run/Button").GetComponent<Button>();
        pickupButton = layout.Find("PickUp/Button").GetComponent<Button>();
        attackButton = layout.Find("Attack/Button").GetComponent<Button>();
        jumpButton = layout.Find("Jump/Button").GetComponent<Button>();

        runText = runButton.GetComponentInChildren<TMP_Text>();
        pickupText = pickupButton.GetComponentInChildren<TMP_Text>();
        attackText = attackButton.GetComponentInChildren<TMP_Text>();
        jumpText = jumpButton.GetComponentInChildren<TMP_Text>();

        runButton.onClick.AddListener(() => StartKeyBinding("Run"));
        pickupButton.onClick.AddListener(() => StartKeyBinding("PickUp"));
        attackButton.onClick.AddListener(() => StartKeyBinding("Attack"));
        jumpButton.onClick.AddListener(() => StartKeyBinding("Jump"));


        SetButtonLabel(runText, RunKey, KeyCode.LeftShift);
        SetButtonLabel(pickupText, PickUpKey, KeyCode.E);
        SetButtonLabel(attackText, AttackKey, KeyCode.Mouse0);
        SetButtonLabel(jumpText, JumpKey, KeyCode.Space);
    }

    void Update()
    {
        if (waitingForKey != null)
        {
            foreach (KeyCode code in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(code))
                {
                    SaveKey(waitingForKey, code);
                    waitingForKey = null;
                    break;
                }
            }
        }
    }

    void StartKeyBinding(string action)
    {
        waitingForKey = action;
        Debug.Log("Очікується клавіша для: " + action);

        switch (action)
        {
            case "Run": runText.text = "<Waiting...>"; break;
            case "PickUp": pickupText.text = "<Waiting...>"; break;
            case "Attack": attackText.text = "<Waiting...>"; break;
            case "Jump": jumpText.text = "<Waiting...>"; break;
        }
    }

    void SaveKey(string action, KeyCode key)
    {
        string keyPref = "Key_" + action;
        PlayerPrefs.SetString(keyPref, key.ToString());
        PlayerPrefs.Save();

        FindAnyObjectByType<CharacterScript>()?.ReloadKeys();
        FindAnyObjectByType<GoalInteraction>()?.ReloadKeys();

        Debug.Log($"Призначено {action}: {key}");

        switch (action)
        {
            case "Run": runText.text = key.ToString(); break;
            case "PickUp": pickupText.text = key.ToString(); break;
            case "Attack": attackText.text = key.ToString(); break;
            case "Jump": jumpText.text = key.ToString(); break;
        }
    }

    void SetButtonLabel(TMP_Text text, string key, KeyCode defaultKey)
    {
        string stored = PlayerPrefs.GetString(key, defaultKey.ToString());
        text.text = stored;
    }
}