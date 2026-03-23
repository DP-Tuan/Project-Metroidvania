using System.Collections;
using UnityEngine;

public class FallingRockTrap : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private Rigidbody2D rb;

    [Header("Shake Settings")]
    [SerializeField] private float shakeDuration = 1.0f;
    [SerializeField] private float shakeIntensity = 0.05f;

    [Header("Fall Settings")]
    [SerializeField] private float fallGravity = 3.0f;
    [SerializeField] private LayerMask groundLayer;
    [Header("Trigger Settings")]
    [SerializeField] private string playerTag = "Player";

    [Header("Damage")][SerializeField] private float damageAmount = 1f;

    private bool hasTriggered = false;
    private Vector3 originalPosition;

    private void Start()
    {
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        originalPosition = transform.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasTriggered && collision.CompareTag(playerTag))
        {
            hasTriggered = true;
            AudioManager.instance.earthquakeSource.enabled = true;
            StartCoroutine(ActivateTrap());
        }
    }

    private IEnumerator ActivateTrap()
    {
        Debug.Log("Trap shaking!");
        float shakeTimer = 0f;
        CameraSmoothDamp.instance.Shake(shakeDuration, 0.08f);
        while (shakeTimer < shakeDuration)
        {
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * shakeIntensity);
            shakeTimer += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition;
        Debug.Log("Trap falling!");
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = fallGravity;
    }


    /**
     * Hàm này ðý?c g?i khi Collider v?t l? c?a ðá
     * (cái KHÔNG ph?i "Is Trigger") va ch?m v?i th? khác.
     */
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!hasTriggered) return;
        if (collision.gameObject.CompareTag(playerTag))
        {
            Debug.Log("Rock hit Player!");

            PlayerDamageReceiver.instance.TakeDamage(damageAmount);

            HandleImpact();
            return;
        }

        if ((groundLayer.value & (1 << collision.gameObject.layer)) > 0)
        {
            Debug.Log("Rock hit the ground.");

            HandleImpact();
        }
    }

    private void HandleImpact()
    {


        AudioManager.instance.earthquakeSource.enabled = false;
        Destroy(gameObject);
    }
}