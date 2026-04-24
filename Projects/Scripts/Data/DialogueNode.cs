using UnityEngine;

[CreateAssetMenu(menuName = "IRAI/Dialogue Node")]
public class DialogueNode : ScriptableObject
{
    public string nodeId;
    public DialogueLine[] lines;
    public ChoiceOption[] choices;       // empty = just dialogue
    public DialogueNode nextIfNoChoice;   // chain
}