using UnityEngine;

public class graveContinue : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        GetComponent<SceneManagerFade>().LoadScene("LevelChanged");
        Debug.Log("GraveContinue");
    }
}
