using UnityEngine;

public class graveContinue : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject open;
    [SerializeField] private PlayerMovementRB playerMovement;
    [SerializeField] private PlayerCameraLook playerCameraLook;

    public void Interact()
    {
        playerMovement.CanMove = false;
        playerCameraLook.SetCursorLock(false);

        Time.timeScale = 0f;
        open.SetActive(true);
    }

    public void CloseDialogue()
    {
        playerMovement.CanMove = true;
        playerCameraLook.SetCursorLock(true);

        Time.timeScale = 1f;
        open.SetActive(false);
    }
    public void LoadScene()
    {
        GetComponent<SceneManagerFade>().LoadScene("LevelChanged");
        Debug.Log("GraveContinue");
    }
}
