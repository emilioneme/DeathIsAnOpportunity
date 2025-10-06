using System;
using System.Collections.Generic;
using UnityEngine;

public class DeathInteractionManager : MonoBehaviour, IInteractable
{ 
    [SerializeField] private DialogueBatch[] dialogues;
    private int batchCurrent = 0;
    private int indexCurrent = 0;
    [SerializeField] private TimelineTracker tracker;
    [SerializeField] private DialogueManager manager;

    private void Awake()
    {

        foreach (DialogueBatch batch in dialogues)
        {
            if (tracker.IsEventCompleted(batch.batchName) && !batch.defaultState)
            {
                batchCurrent++;
                return;
            }
        }

        // potentially could add in an event checker using the TimelingTracker
        // would need to work out the events
        
    }
    public void Interact()
    {
        if (!manager.IsDialoguePlaying)
        {
            DialogueBatch batch = dialogues[batchCurrent];
            NpcDialogue dialogue;

            if (tracker.IsEventCompleted(batch.batchName))
            {
                indexCurrent = batch.dialogues.Length - 1;
                dialogue = batch.dialogues[indexCurrent];
            }
            else 
            { 
                dialogue = batch.dialogues[indexCurrent]; 
            }

            
            if (dialogue.isRepeatable)
            {
                manager.StartDialogue(dialogue);
                if (!batch.defaultState)
                    tracker.MarkEventCompleted(batch.batchName);
                else tracker.MarkEventCompleted(batch.batchName+"Repeating");
            }
            else
            {
                manager.StartDialogue(dialogue);
                if (indexCurrent < batch.dialogues.Length) indexCurrent++;
            }
        }
    }
}
