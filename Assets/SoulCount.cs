using UnityEngine;
using TMPro;

public class SoulCount : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        text.text = gameManager.soulCount.ToString();
    }
}
