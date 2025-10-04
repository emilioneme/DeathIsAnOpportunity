using System.Diagnostics.CodeAnalysis;
using UnityEngine;

[CreateAssetMenu(fileName = "NpcDialogue", menuName = "Scriptable Objects/NpcDialogue")]
public class NpcDialogue : ScriptableObject
{
    public DialogueLine[] lines;
}
