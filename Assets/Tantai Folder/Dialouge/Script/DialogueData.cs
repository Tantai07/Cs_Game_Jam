using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue/NPC Dialogue")]
public class DialogueData : ScriptableObject
{
    [TextArea] public string[] firstTimeDialogues;
    [TextArea] public string[] repeatDialogues;
}
