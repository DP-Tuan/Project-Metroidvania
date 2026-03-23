using UnityEngine;
public class NPCInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject interactionPrompt; public GameObject dialoguePanel; public GameObject dialogueCamera;

    [Header("Dialogue Content")]
    [TextArea(3, 10)] public string[] dialogueLines;
    private bool isPlayerInRange = false;
    public DialogueManager dialogueManager;

    public LayerMask player;
    [SerializeField] private SpriteRenderer npcSpriteRenderer;

    void Start()
    {
        npcSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        if (npcSpriteRenderer == null)
        {
            Debug.LogWarning("NPC " + gameObject.name + " không có SpriteRenderer ð? làm avatar.", this);
        }

        interactionPrompt.SetActive(false);
        dialoguePanel.SetActive(false);
        dialogueCamera.SetActive(false);

        dialogueManager = dialoguePanel.GetComponent<DialogueManager>();
        if (dialogueManager == null)
        {
            Debug.LogError("Không t?m th?y DialogueManager trên Dialogue Panel!");
        }
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {

            if (!dialoguePanel.activeInHierarchy)
            {
                StartDialogue();
            }
        }
    }

    private void StartDialogue()
    {
        dialoguePanel.SetActive(true);
        dialogueCamera.SetActive(true);
        interactionPrompt.SetActive(false);
        dialogueManager.dialogueActive = true;
        SpriteRenderer avatarSprite = (npcSpriteRenderer != null) ? npcSpriteRenderer : null;
        dialogueManager.StartDialogue(dialogueLines);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (player == (player | (1 << collision.gameObject.layer)))
        {
            isPlayerInRange = true;
            interactionPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player == (player | (1 << collision.gameObject.layer)))
        {
            isPlayerInRange = false;
            interactionPrompt.SetActive(false);
            dialogueManager.EndDialogue();
        }
    }
    private void OnTriggerEnter(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {


    }
}