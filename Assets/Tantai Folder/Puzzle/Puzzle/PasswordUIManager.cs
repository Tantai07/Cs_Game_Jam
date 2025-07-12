using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class PasswordUIManager : MonoBehaviour
{
    public static PasswordUIManager Instance;

    public GameObject panel;
    public TMP_InputField inputField;
    public Button confirmButton;

    private NPCInteract currentNPC;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    public void Open(NPCInteract npc)
    {
        currentNPC = npc;
        panel.SetActive(true);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    public void Close()
    {
        panel.SetActive(false);
    }

    private void Start()
    {
        confirmButton.onClick.AddListener(CheckPassword);
        Close();
    }

    public void CheckPassword()
    {
        if (currentNPC == null) return;

        string userInput = inputField.text.Trim();

        if (userInput == currentNPC.correctPassword)
        {
            currentNPC.OnPasswordCorrect(); // แจ้งกลับไปยัง NPC
            Close();
        }
        else
        {
            Debug.Log("Wrong password!");
            inputField.text = "";
        }
    }
}
