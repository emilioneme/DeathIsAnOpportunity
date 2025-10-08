using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathInteractionManager : MonoBehaviour, IInteractable
{ 
    [SerializeField] private DialogueBatch[] dialogues;
    private int batchCurrent = 0;
    private int indexCurrent = 0;
    [SerializeField] private TimelineTracker tracker;
    [SerializeField] private DialogueManager manager;

    private void Start()
    {
        Debug.Log("Awakened");
        foreach (DialogueBatch batch in dialogues)
        {
            Debug.Log(batchCurrent.ToString()+" First");
            Debug.Log(batch.batchName);
            Debug.Log(tracker.IsEventCompleted(batch.batchName));
            if (tracker.IsEventCompleted(batch.batchName))
            {
                Debug.Log("it's running");
                if (batchCurrent != dialogues.Length - 1) batchCurrent++;
                Debug.Log(batchCurrent);
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
            }
            else
            {
                manager.StartDialogue(dialogue);
                if (indexCurrent < batch.dialogues.Length) indexCurrent++;
                if (!batch.defaultState)
                    tracker.MarkEventCompleted(batch.batchName);
                else tracker.MarkEventCompleted(batch.batchName+"Repeating");
            }
        }
    }
}
