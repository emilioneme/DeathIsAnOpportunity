using System;
using UnityEngine;

[CreateAssetMenu(fileName = "DialogueBatch", menuName = "Scriptable Objects/DialogueBatch")]
public class DialogueBatch : ScriptableObject
{
    public string batchName;
    public Boolean defaultState;
    public NpcDialogue[] dialogues;
}
