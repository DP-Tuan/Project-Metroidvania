using UnityEngine;

public class CameraSmoothDamp : MonoBehaviour
{
    public static CameraSmoothDamp instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform target;

    public float smoothTime = 0.3f;
    public Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    [Header("Clamping (Gi?i h?n Camera)")]
    public bool clampXPosition = true;
    public float minXPosition = 0f;
    public float maxXPosition = 20f;

    [Header("Clamp Y")]
    public bool clampYPosition = false;
    public float minYPosition = -5f;
    public float maxYPosition = 10f;

    [Header("Vertical Look Settings")]
    public float lookDownAmount = 5f; public float lookDownSpeed = 3f;
    private float currentYOffset = 0f;
    private float targetYOffset = 0f;

    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.25f;

    void LateUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("Camera target is not assigned!");
            return;
        }

        bool lookingDown = Input.GetAxisRaw("Vertical") < 0; bool falling = false;

        Rigidbody2D rb = target.GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.y < -0.1f)
        {
            falling = true;
        }

        if (lookingDown || falling)
            targetYOffset = -lookDownAmount;
        else
            targetYOffset = 0f;

        currentYOffset = Mathf.Lerp(currentYOffset, targetYOffset, Time.deltaTime * lookDownSpeed);

        Vector3 targetPosition = target.position + offset + new Vector3(0f, currentYOffset, 0f);

        if (clampXPosition)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minXPosition, maxXPosition);
        }

        if (clampYPosition)
        {
            targetPosition.y = Mathf.Clamp(targetPosition.y, minYPosition, maxYPosition);
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);

        if (shakeDuration > 0)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeMagnitude;
            shakeOffset.z = 0;
            transform.position += shakeOffset;

            shakeDuration -= Time.deltaTime;
        }
    }

    public void Shake(float duration, float magnitude = 0.25f)
    {
        shakeDuration = duration;
        shakeMagnitude = magnitude;
    }
}