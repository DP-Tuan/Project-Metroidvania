using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    public BossAI bossAI;

    public float maxHealth = 100f;

    [SerializeField] private float currentHealth;
    public float CurrentHealth { get => currentHealth; set => Mathf.Clamp(value, 0, maxHealth); }

    private bool isPhase2Active = false;
    private const float PHASE_2_THRESHOLD = 0.40f;
    public Animator animator;

    private void Awake()
    {
        this.animator = GetComponent<Animator>();
    }

    void Start()
    {
        currentHealth = maxHealth;
        if (bossAI == null)
        {
            bossAI = GetComponentInParent<BossAI>();
        }
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        currentHealth = Mathf.Max(currentHealth, 0);
        animator.SetTrigger("Hurt");
        AudioManager.instance.PlaySFX(AudioManager.instance.enemyHurt);
        if (!isPhase2Active && (currentHealth / maxHealth) <= PHASE_2_THRESHOLD)
        {
            isPhase2Active = true;
            Debug.Log("Boss chuy?n sang Giai ðo?n 2!");
            bossAI.ActivatePhase2();
        }

        if (currentHealth <= 0)
        {
            Die();
        }

        Debug.Log("Boss nh?n sát thýõng. Máu c?n l?i: " + currentHealth);
    }

    void Die()
    {
        Debug.Log("Boss ð? b? ðánh b?i!");
        Destroy(gameObject);
    }
}