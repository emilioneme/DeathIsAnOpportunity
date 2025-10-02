using System;
using UnityEngine;

public class DeathInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private CutsceneManager cutsceneManager;
    public void Interact()
    {
        cutsceneManager.Play();
    }
}
