using System;
using UnityEngine;

public class DeathInteractionJoseph : MonoBehaviour, IInteractable
{ 
    [SerializeField] private CutsceneManager cutsceneManager;
    public void Interact()
    {
        cutsceneManager.Play();
    }
}
