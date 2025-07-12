using UnityEngine;

public class NPCInteract : MonoBehaviour
{
    public string npcName = "???";
    public DialogueData dialogueData;
    public NPCType npcType;

    public float detectRadius = 1.5f;
    public LayerMask playerLayer;
    public GameObject hintE_UI;

    private bool isPlayerNearby = false;
    private bool hasTalked = false;
    private bool isTalking = false;

    private string[] currentDialogue;
    private int dialogueIndex = 0;
    private void Start()
    {
        playerLayer = LayerMask.GetMask("Default");
        hintE_UI = transform.Find("Text").gameObject;
    }
    private void Update()
    {
        DetectPlayer();

        if (isPlayerNearby && !isTalking && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame)
        {
            Player_Movement.Instance.AddStress(30);
            hintE_UI.SetActive(false);
            StartDialogue();
        }

        if (isPlayerNearby && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EndDialogue();
        }
    }

    void DetectPlayer()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, detectRadius, playerLayer);
        isPlayerNearby = (hit != null && hit.CompareTag("Player"));

        if (hintE_UI != null && !isTalking)
            hintE_UI.SetActive(isPlayerNearby);
    }

    void StartDialogue()
    {
        isTalking = true;
        dialogueIndex = 0;

        currentDialogue = hasTalked ? dialogueData.repeatDialogues : dialogueData.firstTimeDialogues;

        DialogueManager.Instance.OnDialogueComplete = ContinueDialogue;

        if (currentDialogue.Length > 0)
            DialogueManager.Instance.ShowDialogue(npcName, currentDialogue[dialogueIndex]);
    }

    void ContinueDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < currentDialogue.Length)
        {
            DialogueManager.Instance.ShowDialogue(npcName, currentDialogue[dialogueIndex]);
        }
        else
        {
            EndDialogue();

            if (!hasTalked && npcType == NPCType.Extrovert)
            {
                Player_Movement.Instance.FindFriend();
            }

            hasTalked = true;
        }
    }

    void EndDialogue()
    {
        isTalking = false;
        dialogueIndex = 0;
        DialogueManager.Instance.HideDialogue();
        DialogueManager.Instance.OnDialogueComplete = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRadius);
    }
}

public enum NPCType
{
    Extrovert,
    Introvert,
    Item
}
