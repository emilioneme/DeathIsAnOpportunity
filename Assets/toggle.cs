using UnityEngine;

public class toggle : MonoBehaviour
{
    [SerializeField] private GameObject panel;

    private void Start()
    {
        panel.SetActive(!panel.activeSelf);
    }
    public void TogglePanel()
    {
        panel.SetActive(!panel.activeSelf);
    }
}
