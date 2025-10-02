using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cutscene
{
    [SerializeField] private List<DialogueLines> dialogue = new List<DialogueLines>();
    int dialogueIndex = 0;
    public bool isFinished = false;

    public string getLine()
    {
        return (dialogueIndex < dialogue.Count) ? dialogue[dialogueIndex++].text+'\n' : null;
    }
}
