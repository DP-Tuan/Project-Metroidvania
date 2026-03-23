using System.Collections;
using UnityEngine;

public class PlayerDamageReceiver : MonoBehaviour
{
    public static PlayerDamageReceiver instance;
    [SerializeField] protected float health;
    [SerializeField] protected float maxHealth = 1;
    public float MaxHealth { get => maxHealth; set => maxHealth = value; }
    [SerializeField] protected PlayerController controller;

    public float Health { get => this.health; set => this.health = Mathf.Clamp(value, 0, maxHealth); }

    [Header("Flash")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    public Material flashMaterial;
    public Material defaultMaterial;
    private Coroutine flashRoutine;

    private void Start()
    {
        instance = this;
        this.health = this.maxHealth;
        controller = transform.parent.GetComponent<PlayerController>();
        if (spriteRenderer == null)
            spriteRenderer = controller.GetComponentInChildren<SpriteRenderer>();

        defaultMaterial = spriteRenderer.material;
    }

    public virtual void TakeDamage(float damage)
    {
        this.health -= damage;
        AudioManager.instance.PlaySFX(AudioManager.instance.playerHurt);
        this.health = Mathf.Clamp(health, 0, maxHealth);
        if (health > 1)
            controller._animation.SetTrigger("Hurt");
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(Flash());
        if (health <= 0)
        {
            StartCoroutine(PlayerDie());
        }
    }

    private IEnumerator Flash()
    {
        spriteRenderer.material = flashMaterial; yield return new WaitForSeconds(0.1f); spriteRenderer.material = defaultMaterial;
    }

    public IEnumerator PlayerDie()
    {

        controller._animation.SetTrigger("Dead");
        controller.canMove = false;
        yield return new WaitForSeconds(1f);
        Destroy(transform.parent.gameObject);
        UIManager.Instance.Lose();

    }
}
