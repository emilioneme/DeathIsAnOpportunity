using System.Collections;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI Elements")]
    public PlayerUI playerUI;
    public GameObject dialoguePanel;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;

    private AudioSource voiceSource;
    private AudioSource prevVoice;
    
    [SerializeField]
    private PlayerMovementRB playerMovement;
    [SerializeField]
    private float typingSpeed = 0.05f;
    public bool IsDialoguePlaying { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    public void StartDialogue(NpcDialogue dialogue)
    {
        IsDialoguePlaying = true;
        dialoguePanel.SetActive(true);
        playerMovement.CanMove = false;
        StartCoroutine(RunDialogue(dialogue));
    }

    private IEnumerator RunDialogue(NpcDialogue dialogue)
    {
        yield return new WaitForSeconds(0.2f);

        foreach (var line in dialogue.lines)
        {
            // Show speaker
            nameText.text = line.speakerName;

            // Clear previous text
            dialogueText.text = "";

            // Play voice if exists
            if (line.voiceClip != null)
            {
                voiceSource.Stop();
                voiceSource.clip = line.voiceClip;
                voiceSource.Play();
                prevVoice = voiceSource;
            }
            else if (prevVoice != null)
            {
                voiceSource.Stop();
                prevVoice = null;
            }
            // iterate through each character of the dialogue line
            foreach (char c in line.text)
            {
                bool end = false;
                dialogueText.text += c;

                float timer = 0f;
                while (timer < typingSpeed)
                {
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        end = true;
                        dialogueText.text = line.text;
                        break;
                    }
                    timer += Time.deltaTime;
                    yield return null; // wait 1 frame
                }
                if (end) break;
            }

            yield return new WaitForSeconds(0.1f);

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));

            yield return new WaitForSeconds(0.1f);
        }

        EndDialogue();
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        playerMovement.CanMove = true;
        nameText.text = "";
        dialogueText.text = "";
        IsDialoguePlaying = false;
        playerUI.interacted = false;
    }
}
