using System;
using TMPro;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject prompt;
    [SerializeField] float interactRange;

    public Boolean interacted;
    private void Awake()
    {
        interacted = false;
    }
    void Update()
    {
        Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactRange) && !interacted)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();
            if (interactable != null)
            {
                prompt.SetActive(true);
                return;
            }
        }

        // If no interactable detected
        prompt.SetActive(false);
    }
}
