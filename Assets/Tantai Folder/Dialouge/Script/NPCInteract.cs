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
    [SerializeField]private bool hasTalked = false;
    private bool isTalking = false;

    public bool useCondition = false;
    public ConditionType condition;
    public int conditionValue = 1;

    private string[] currentDialogue;
    private int dialogueIndex = 0;

    [Header("Password Condition")]
    public string correctPassword = "1234";
    private bool passwordVerified = false;

    [Header("Item Reward")]
    public bool givesItem = false;
    public Item itemToGive;
    public bool onlyGiveOnce = true;
    private bool hasGivenItem = false;


    private void Start()
    {
        playerLayer = LayerMask.GetMask("Default");
        hintE_UI = transform.Find("Text").gameObject;
    }
    private void Update()
    {
        DetectPlayer();

        if (isPlayerNearby && !isTalking && UnityEngine.InputSystem.Keyboard.current.eKey.wasPressedThisFrame && Player_Movement.Instance.stress != Player_Movement.Instance.maxStress)
        {
            Player_Movement.Instance.AddStress(10);
            hintE_UI.SetActive(false);
            StartDialogue();
        }

        if (isPlayerNearby && UnityEngine.InputSystem.Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            EndDialogue();
        }
    }
    void GiveItemToPlayer()
    {
        if (!givesItem) return;
        if (onlyGiveOnce && hasGivenItem) return;

        bool success = InventoryManager.Instance.AddItem(itemToGive);
        if (success)
        {
            hasGivenItem = true;
        }
        else
        {
            Player_Movement.Instance.Show_Inventory_Full();
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
        if (useCondition && condition == ConditionType.Password && !passwordVerified)
        {
            PasswordUIManager.Instance.Open(this);
            return;
        }

        isTalking = true;
        dialogueIndex = 0;

        currentDialogue = (useCondition && IsConditionMet())
            ? dialogueData.conditionalDialogues
            : (hasTalked ? dialogueData.repeatDialogues : dialogueData.firstTimeDialogues);

        DialogueManager.Instance.OnDialogueComplete = ContinueDialogue;

        if (currentDialogue.Length > 0)
            DialogueManager.Instance.ShowDialogue(npcName, currentDialogue[dialogueIndex]);
    }

    // เรียกเมื่อรหัสถูก
    public void OnPasswordCorrect()
    {
        passwordVerified = true;
        StartDialogue();
    }

    bool IsConditionMet()
    {
        switch (condition)
        {
            case ConditionType.HasFriend:
                return Player_Movement.Instance.friendFound >= conditionValue;

            case ConditionType.HasItem:
                return InventoryManager.Instance.HasItem("Teddy Bear");

            case ConditionType.Password:
                return passwordVerified;

            default:
                return false;
        }
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
                Player_Movement.Instance.AddStress(30);
                Player_Movement.Instance.FindFriend();
            }

            GiveItemToPlayer();
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
public enum ConditionType
{
    HasFriend,
    HasItem,
    Password
}
