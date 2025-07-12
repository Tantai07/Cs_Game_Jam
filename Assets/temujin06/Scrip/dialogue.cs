using UnityEngine;
using UnityEngine.InputSystem;

public class dialogue : MonoBehaviour
{
    public string[] dialogueLines; // ข้อความพูดทีละบรรทัด
    private bool playerInRange = false;
    private bool isTalking = false;
    private int currentLine = 0;

    public GameObject dialogueUI;
    public TMPro.TextMeshProUGUI dialogueText;

    void Update()
    {
        if (playerInRange && Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (!isTalking)
            {
                StartDialogue();
            }
            else
            {
                NextLine();
            }
        }
    }

    void StartDialogue()
    {
        isTalking = true;
        currentLine = 0;
        dialogueUI.SetActive(true);
        dialogueText.text = dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;
        if (currentLine < dialogueLines.Length)
        {
            dialogueText.text = dialogueLines[currentLine];
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueUI.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (isTalking) EndDialogue();
        }
    }
}
