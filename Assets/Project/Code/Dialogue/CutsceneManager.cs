using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private List<Cutscene>cutscenes;
    [SerializeField] private PlayerMovementRB playerMovement;
    int cutsceneIndex = 0;
    public TextMeshProUGUI textComponent;
    public float textSpeed;
    public GameObject textBox;
    
    public void Play()
    {
        textBox.active = cutsceneIndex  == cutscenes.Count ? false : true;
        bool finished = cutsceneIndex == cutscenes.Count;

        textBox.SetActive(!finished); // fixed deprecated "active"

        if (!finished)
        {
            playerMovement.CanMove = false; // freeze
            StartCoroutine(TypeLine());
        }
        else
        {
            cutsceneIndex = 0;
            playerMovement.CanMove = true; // unfreeze
        }
    }

    IEnumerator TypeLine()
    {
        string line = cutscenes[cutsceneIndex].getLine();

        if (line == null)
        {
            cutsceneIndex++;
            textComponent.text = string.Empty;
        }
        else
        {
            foreach (char c in line.ToCharArray())
            {
                textComponent.text += c;
                yield return new WaitForSeconds(textSpeed);
            }
        }
    }

    
    
}
