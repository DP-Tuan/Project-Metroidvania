using UnityEngine;

public class BossDamageOnContact : MonoBehaviour
{
    public int damage = 15;
    public BossAI bossAI;

    public float damageInterval = 0.4f;
    private float nextDamageTime;

    private void Start()
    {
        this.bossAI = GetComponent<BossAI>();
    }

    private void TryDamage(Collider2D collision)
    {
        if (!bossAI.IsAttacking) return;
        if (!collision.CompareTag("Player")) return;

        if (Time.time >= nextDamageTime)
        {
            nextDamageTime = Time.time + damageInterval;
            PlayerDamageReceiver.instance.TakeDamage(damage);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TryDamage(collision);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TryDamage(collision);
    }
}
