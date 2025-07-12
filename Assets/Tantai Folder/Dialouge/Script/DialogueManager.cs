using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueManager : MonoBehaviour, IPointerClickHandler
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public GameObject inventory;
    public TMP_Text npcNameText;
    public TMP_Text dialogueText;
    public float typingSpeed = 0.03f;

    private string currentText = "";
    private Coroutine typingCoroutine;
    public System.Action OnDialogueComplete;
    private bool isTyping = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        inventory = GameObject.Find("Canvas_Equipment");
    }
    public void ShowDialogue(string npcName, string message)
    {
        dialoguePanel.SetActive(true);
        Player_Movement.Instance.canMove = false;
        inventory.SetActive(false);

        if (npcNameText != null)
            npcNameText.text = npcName;

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(message));
    }

    public void HideDialogue()
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        dialoguePanel.SetActive(false);
        Player_Movement.Instance.canMove = true;
        inventory.SetActive(true);
        dialogueText.text = "";
        if (npcNameText != null) npcNameText.text = "";
        isTyping = false;
    }

    IEnumerator TypeText(string message)
    {
        dialogueText.text = "";
        currentText = message;
        isTyping = true;

        foreach (char letter in message.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueText.text = currentText;
            isTyping = false;
        }
        else
        {
            OnDialogueComplete?.Invoke();
        }
    }
}
