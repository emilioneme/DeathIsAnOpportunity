using UnityEngine;

public class graveContinue : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject open;
    [SerializeField] private PlayerMovementRB playerMovement;
    [SerializeField] private PlayerCameraLook playerCameraLook;
    [SerializeField] private PlayerUI playerUI;
    public void Interact()
    {
        playerMovement.CanMove = false;
        playerCameraLook.SetCursorLock(false);


        open.SetActive(true);
    }

    public void CloseDialogue()
    {
        playerMovement.CanMove = true;
        playerCameraLook.SetCursorLock(true);
        playerUI.interacted = false;

        open.SetActive(false);
    }
    public void LoadScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("LevelChanged");
        Debug.Log("GraveContinue");
    }
}
