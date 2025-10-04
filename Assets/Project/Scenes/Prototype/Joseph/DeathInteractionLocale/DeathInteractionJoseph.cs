using System;
using UnityEngine;

public class DeathInteractionJoseph : MonoBehaviour, IInteractable
{ 
    [SerializeField] private NpcDialogue dialogue;
    public void Interact()
    {
        if (!DialogueManager.Instance.IsDialoguePlaying)
        {
            DialogueManager.Instance.StartDialogue(dialogue);
        }
    }
}
