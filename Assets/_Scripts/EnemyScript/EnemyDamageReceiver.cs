using System.Collections;
using UnityEngine;

public class EnemyDamageReceiver : MonoBehaviour
{
    public float health;
    public float maxHealth = 5;
    [SerializeField] protected EnemyBehaviour controller;
    [SerializeField] protected Animator _animation;

    [Header("Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Material flashMaterial;
    public Material defaultMaterial;
    private Coroutine flashRoutine;
    private void Awake()
    {
        controller = GetComponentInParent<EnemyBehaviour>();
        _animation = controller.GetComponentInChildren<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = controller.GetComponentInChildren<SpriteRenderer>();

        defaultMaterial = spriteRenderer.material;
    }

    private void Start()
    {
        health = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health > 1)
            _animation.SetTrigger("Hurt");
        Debug.Log("Take Damage");
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(Flash());
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial; yield return new WaitForSeconds(0.1f); spriteRenderer.material = defaultMaterial;
    }

    IEnumerator Die()
    {
        _animation.SetTrigger("Dead");

        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
    }
}
