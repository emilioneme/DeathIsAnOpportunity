using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDialogue", menuName = "Scriptable Objects/NpcDialogue")]
public class NpcDialogue : ScriptableObject
{
    public string dialogueId;
    public bool isRepeatable;
    public DialogueLine[] lines;

}
