using TMPro;
using UnityEngine;

public class PlayerTeleport : MonoBehaviour
{
    private GameObject currentTeleporter;
    [SerializeField] TMP_Text text;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTeleporter != null)
            {
                AudioManager.instance.PlaySFX(AudioManager.instance.teleportClip);
                transform.position = currentTeleporter.GetComponent<Teleporter>().GetDestination().position;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            currentTeleporter = collision.gameObject;
            text = currentTeleporter.GetComponentInChildren<TMP_Text>();
            text.enabled = true;
            currentTeleporter = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Teleporter"))
        {
            text = collision.gameObject.GetComponentInChildren<TMP_Text>();
            if (collision.gameObject == currentTeleporter)
            {
                currentTeleporter = null;
            }
            text.enabled = false;
        }
    }
}