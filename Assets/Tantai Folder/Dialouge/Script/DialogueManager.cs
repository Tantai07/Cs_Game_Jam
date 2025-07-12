using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DialogueManager : MonoBehaviour, IPointerClickHandler
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public TMP_Text npcNameText;      // <== เพิ่ม
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

    public void ShowDialogue(string npcName, string message)
    {
        dialoguePanel.SetActive(true);

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
