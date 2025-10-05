using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class IntoTheFray : MonoBehaviour
{
    [SerializeField] private string nextSceneName = "MainScene";

    // --- If you're using the new Input System ---
    public void OnInteract(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            LoadNextScene();
        }
    }

    // --- If you're using old Input.GetKeyDown ---
    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.eKey.wasPressedThisFrame)
        {
            LoadNextScene();
        }
        // OR: if (Input.GetKeyDown(KeyCode.E)) LoadNextScene();
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
