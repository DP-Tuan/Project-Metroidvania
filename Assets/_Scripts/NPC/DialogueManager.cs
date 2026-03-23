using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("UI Components")]
    public TextMeshProUGUI dialogueText; public GameObject dialogueCamera;
    public SpriteRenderer npcAvatarImage;

    private string[] currentDialogue;
    private int dialogueIndex = 0;
    private PlayerController controller;
    public bool dialogueActive = false;
    void Awake()
    {
        controller = FindAnyObjectByType<PlayerController>();
        if (controller == null)
        {
            Debug.LogError("Không t?m th?y PlayerMovement trong Scene!");
        }
    }

    void Update()
    {
        if (dialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextLine();
        }
    }

    public void StartDialogue(string[] dialogue)
    {
        gameObject.SetActive(true); controller.canMove = false;
        currentDialogue = dialogue;
        dialogueIndex = 0;
        DisplayNextLine();
    }

    public void DisplayNextLine()
    {
        if (dialogueIndex < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[dialogueIndex];
            dialogueIndex++;
            Debug.Log(dialogueIndex);
        }
        else
        {
            EndDialogue();
        }
    }

    public void EndDialogue()
    {
        dialogueActive = false;
        gameObject.SetActive(false); dialogueCamera.SetActive(false);
        controller.canMove = true;
    }
}